using System;

namespace BuckRogers.Networking
{
	public enum NetworkMessages : uint
	{
		ConnectionAcknowledged = 1,
		ServerDisconnected,
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
		ClientReady,
		GameSettings,
		GameStarting,
		GameStarted,


		GameMessagesFirst,
		InitialSetupInformation = GameMessagesFirst,
		PlayerPlacementStarted,
		PlayerPlacementEnded,
		CreateUnits,
		PlayerTurnStarted,
		PlayerTurnEnded,


		GameMessagesLast,

	}
}