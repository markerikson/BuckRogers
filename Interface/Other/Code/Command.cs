using System;
using System.Collections;
using System.Timers;
using System.Windows.Forms;

namespace CommandManagement
{
public class Command
{
    // Members
    private CommandInstanceList commandInstances;
    private CommandManager      manager;
    private string              strTag;
    protected bool              enabled;
    protected bool              check;

    // Constructor
    public Command( string          strTag, 
                    ExecuteHandler  handlerExecute, 
                    UpdateHandler   handlerUpdate)
    {
        commandInstances = new CommandInstanceList(this);
        this.strTag = strTag;
        OnUpdate += handlerUpdate;
        OnExecute += handlerExecute;
    }

    // CommandInstances collection
    public CommandInstanceList CommandInstances
    {
        get 
        {
            return commandInstances;
        }
    }

    // Tag property: Unique internal name for each command
    public string Tag
    {
        get {return strTag; }
    }

    // Manager property: maintain association with parent command manager
    internal CommandManager Manager
    {
        get 
        {
            return manager;
        }
        set
        {
            manager = value;
        }
    }

    public override string ToString()
    {
        return Tag;
    }

    // Methods to trigger events
    public void Execute()
    {
        if (OnExecute != null)
            OnExecute(this);
    }

    internal void ProcessUpdates()
    {
        if (OnUpdate != null)
            OnUpdate(this);
    }

    // Enabled property
    public bool Enabled
    {
        get
        {
            return enabled;
        }
        set
        {
            enabled = value;
            foreach(object instance in commandInstances)
            {
                Manager.GetCommandExecutor(instance).Enable(
                    instance, enabled);
            }
        }
    }

    // Checked property
    public bool Checked
    {
        get
        {
            return check;
        }
        set
        {
            check = value;
            foreach(object instance in commandInstances)
            {
                Manager.GetCommandExecutor(instance).Check(
                    instance, check);
            }
        }
    }

    // Events
    public delegate void UpdateHandler(Command cmd);
    public event UpdateHandler OnUpdate;

    public delegate void ExecuteHandler(Command cmd);
    public event ExecuteHandler OnExecute;


    //
    // Nested collection class
    //
    public class CommandInstanceList : System.Collections.CollectionBase
    {
        internal CommandInstanceList(Command acmd)
            : base()
        {
            command = acmd;
        }

        private Command command;

        public void Add(object instance)
        {
            this.List.Add(instance);
        }

        public void Add(object[] items)
        {
            foreach (object item in items)
            {
                this.Add(item);
            }
        }

        public void Remove(object instance)
        {
            this.List.Remove(instance);
        }

        public object this[int index]
        {
            get
            {
                return this.List[index];
            }
        }

        protected override void OnInsertComplete(   System.Int32    index, 
                                                    System.Object   value)
        {
            command.Manager.GetCommandExecutor(value).InstanceAdded(
                value, command);
        }
    }
}
}
