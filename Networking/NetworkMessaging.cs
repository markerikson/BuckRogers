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
		PlayerPlacementStarted,
		PlayerChoseUnits,
		PlayerPlacedUnits,
		PlayerPlacementEnded,
		NextPlayer,
		CreateUnits,
		PlayerTurnStarted,
		PlayerTurnEnded,


		GameplayMessagesLast,

	}
}