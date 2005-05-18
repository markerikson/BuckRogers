/******************************************************************************
Dictionary
-------------------------------------------------------------------------------
CommandManager          -   Global service that manages a collection of 
                            commands

Command                 -   A conceptual representation for an application
                            operation (ie.  Save, Edit, Load, etc...)
                            
Command Instance        -   A UI element associated with a command (ie, Menu
                            item, Toolbar Item, etc...).  A command can have
                            multiple instances
                            
Command Type            -   A UI class that can house a command Instance (ie
                            "System.Windows.Forms.MenuItem",
                            "System.Windows.Forms.ToolbarItem" )

CommandExecutor         -   An object that can handle all communication between
                            the command manager and a particular command instance
                            for a particular command type.

UpdateHandler           -   Event handler for the Commands Update event.
******************************************************************************/
using System;
using System.Collections;
using System.Timers;
using System.Windows.Forms;

namespace CommandManagement
{
public class CommandManager : System.ComponentModel.Component
{
    // Member Variables
    private CommandsList        commands;
    private Hashtable           hashCommandExecutors;   

    // Constructor
    public CommandManager()
    {
        commands                = new CommandManager.CommandsList(this);
        hashCommandExecutors    = new Hashtable();

        // Setup idle processing
        Application.Idle += new EventHandler(this.OnIdle);

        // By default, menus and toolbars are known
        RegisterCommandExecutor(    "System.Windows.Forms.MenuItem", 
                                    new MenuCommandExecutor());

        RegisterCommandExecutor(    "System.Windows.Forms.ToolBarButton", 
                                    new ToolbarCommandExecutor());

		RegisterCommandExecutor("System.Windows.Forms.Button", new ButtonCommandExecutor());
    }

    // Commands Property: Fetches the Command collection
    public CommandsList Commands
    {
        get
        {
            return commands;
        }
    }

    // Command Executor association methods
    internal void RegisterCommandExecutor(  string          strType, 
                                            CommandExecutor executor)
    {
        hashCommandExecutors.Add(strType, executor);
    }

    internal CommandExecutor GetCommandExecutor(object instance)
    {
        return hashCommandExecutors[instance.GetType().ToString()] 
            as CommandExecutor;
    }


    //  Handler for the Idle application event.
    private void OnIdle(object sender, System.EventArgs args)
    {
        IDictionaryEnumerator myEnumerator = 
            (IDictionaryEnumerator)commands.GetEnumerator();
        while ( myEnumerator.MoveNext() )
        {
            Command cmd = myEnumerator.Value as Command;
            if (cmd != null)
                cmd.ProcessUpdates();
        }
    }

    //
    // CommandsList Collection Implementation
    //
    public class CommandsList : ICollection, IEnumerable
    {
        internal CommandsList(CommandManager amgr)
        {
            cmdmgr = amgr;
            commands = new SortedList();
        }

        private SortedList commands;

        private CommandManager cmdmgr;
        internal CommandManager Manager
        {
            get
            {
                return cmdmgr;
            }
            set
            {
                cmdmgr = value;
            }
        }

        // ICollection interface
        public object SyncRoot
        {
            get
            {
                return commands.SyncRoot;
            }
        }

        public bool IsSynchronized
        {
            get 
            {
                return commands.IsSynchronized;
            }
        }

        public int Count
        {
            get
            {
                return commands.Count;
            }
        }

        public void CopyTo(System.Array array, int index)
        {
            commands.CopyTo(array, index);
        }

        // IEnumerable interface

        public System.Collections.IEnumerator GetEnumerator()
        {
            return commands.GetEnumerator();
        }

        // Commands collection interface

        public void Add(Command command)
        {
            command.Manager = Manager;
            commands.Add(command.ToString(), command);
        }

        public void Remove(string cmdTag)
        {
            commands.Remove(cmdTag);
        }

        public bool Contains(string cmdTag)
        {
            return commands.Contains(cmdTag);
        }

        public Command this[string cmdTag]
        {
            get
            {
                return commands[cmdTag] as Command;
            }
        }
    }
}   // end class CommandManager


}  // end namespace
