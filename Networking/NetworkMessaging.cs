using System;

namespace BuckRogers.Networking
{
	public enum GameMessage : uint
	{
		ConnectionAcknowledged = 1,
		ServerStarted,
		ServerDisconnected,
		ServerStatus,
		PlayerNameRequested,
		PlayerNameUpdated,
		PlayerColorRequested,
		PlayerColorUpdated,
		PublicChatMessage,
		PrivateChatMessage,
		ClientListRequested,
		OtherClientsList,
		OtherClientConnected,
		OtherClientDisconnected,
		LoginCompleted,
		PlayerAdded,
		PlayerListing,
		PlayerRemoved,
		PlayerAccepted,
		PlayerDenied,
		StatusUpdate,
		ClientLoaded,
		ClientReady,
		GameSettings,
		GameStarting,
		GameStarted,


		GameplayMessagesFirst,
		InitialSetupInformation = GameplayMessagesFirst,

		PlacementPhaseStarted,		
		PlayerChoseUnits,
		PlayerPlacedUnits,
		PlacementPhaseEnded,
		
		NextPlayer,
		//StartFirstTurn,

		MovementPhaseStarted,
		PlayerTransportedUnits,
		PlayerMovedUnits,
		PlayerUndidMove,
		PlayerRedidMove,
		PlayerFinishedMoving,
		MovementPhaseEnded,

		ClientReadyForCombat,
		ClientReadyForProduction,

		CombatPhaseStarted,
		CombatPhaseEnded,

		TurnFinished,
		GameplayMessagesLast,

	}
}