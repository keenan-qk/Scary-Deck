using UnityEngine;
using System.Collections.Generic; 

[System.Serializable]
public class RoomConnection
{
	public int roomAIndex;
	public DoorDirection doorA; 

	public int roomBIndex;
	public DoorDirection doorB; 
}

public class RoomManager : MonoBehaviour
{
	[Header("Room Prefabs")]
	[SerializeField] private GameObject[] roomPrefabs;
	private List<int> selectedRooms = new List<int>(); 

	[Header("Player")]
	[SerializeField] private Transform player; 

	[Header("Door Connections")]
	[SerializeField] private RoomConnection[] connections; 

	private GameObject currentRoom;
	private int currentRoomIndex;

	private int bossRoomIndex = 7; 

	[Header("Rooms Per Run")]
	[SerializeField] private int roomsPerRun = 5;

	public static RoomManager Instance { get; private set; }

	private void Awake()
	{
		Instance = this; 
	}

	private void Start()
	{
		if (player == null)
		{
			Debug.LogError("Player reference is missing!"); 
			return;
		}

		bool roomsGenerated = false; 

		for (int attempt = 0; attempt < 51; attempt++)
		{
			GenerateRooms(); 

			if (GenerateConnections())
			{
				roomsGenerated = true; 
				Debug.Log("Rooms generated successfully!");
				break; 
			}
		}

		Debug.Log("Generated room order: " + string.Join(" -> ", selectedRooms.ConvertAll(index => roomPrefabs[index].name))); 
		
		if (!roomsGenerated)
		{	
			Debug.Log("Failed to generated valid rooms!");
			return; 
		}

		// CheckRoomDoors();
		// CheckFirstConnection(); 

		if (selectedRooms.Count > 0)
		{
			LoadRoom(selectedRooms[0]);
		}
	}

	public void TryGoThroughDoor(DoorDirection direction)
	{
		foreach (RoomConnection connection in connections)
		{
			// Travel from Room A to Room B 
			if (currentRoomIndex == connection.roomAIndex && direction == connection.doorA)
			{
				LoadRoom(connection.roomBIndex, connection.doorB); 
				return; 
			}
			// Travel from Room B to Room A 
			if (currentRoomIndex == connection.roomBIndex && direction == connection.doorB)
			{
				LoadRoom(connection.roomAIndex, connection.doorA); 
				return; 
			}
		}
	}

	public void LoadRoom(int roomIndex, DoorDirection? entryDirection = null)
	{

		if (roomIndex < 0 || roomIndex >= roomPrefabs.Length || roomPrefabs[roomIndex] == null) 
		{
			Debug.LogError("Invalid room index!");
			return;
		}

		if (roomPrefabs[roomIndex] == null)
		{
			Debug.LogError("Room prefab is missing!");
			return;
		}

		GameObject newRoom = Instantiate(roomPrefabs[roomIndex], Vector3.zero, Quaternion.identity);

		RoomDoor entryDoor = null; 

		if (entryDirection.HasValue)
		{
			foreach (RoomDoor door in newRoom.GetComponentsInChildren<RoomDoor>(true))
			{
				if (door.Direction == entryDirection.Value)
				{
					entryDoor = door;
					break;
				}
			}

			if (entryDoor == null || entryDoor.SpawnPoint == null)
			{
				Debug.LogError("Entry doorway or spawn point missing!");
				Destroy(newRoom);
				return; 
			}
		}

		if (currentRoom != null)
		{
			currentRoom.SetActive(false);
			Destroy(currentRoom);
		}

		currentRoom = newRoom;
		currentRoomIndex = roomIndex;
		ConfigureRoomDoors(currentRoom, currentRoomIndex); 

		if (entryDoor != null)
		{
			player.position = entryDoor.SpawnPoint.position;
		}

	}

	public void GenerateRooms()
	{
		selectedRooms.Clear();

		if (roomPrefabs == null || roomPrefabs.Length < 3)
		{
			Debug.LogError("At least 3 room prefabs are required!");
			return; 
		}

		// Always starting with Room_01
		selectedRooms.Add(0);

		// Keep selecting rooms until we have 3
		while (selectedRooms.Count < roomsPerRun)
		{
			int randomIndex = Random.Range(1, roomPrefabs.Length); 

			// Not selecting the same room twice
			if (!selectedRooms.Contains(randomIndex))
			{
				selectedRooms.Add(randomIndex);
			}
		}

		// Printing the selected rooms
		Debug.Log("Generated Rooms: ");

		foreach (int roomIndex in selectedRooms)
		{
			Debug.Log(roomPrefabs[roomIndex].name); 
		}

		selectedRooms.RemoveAll(index => index == bossRoomIndex); 
		selectedRooms.Add(bossRoomIndex);
	}

	public void CheckRoomDoors()
	{
		foreach (int roomIndex in selectedRooms)
		{
			GameObject roomPrefab = roomPrefabs[roomIndex]; 

			RoomDoor[] doors = roomPrefab.GetComponentsInChildren<RoomDoor>(true); 

        		Debug.Log("Checking doors for: " + roomPrefab.name);

			foreach (RoomDoor door in doors)
			{
				Debug.Log("Available door: " + door.Direction); 
			}
		}
	}

	private DoorDirection GetOppositeDirection(DoorDirection direction)
	{
		switch (direction)
		{
			case DoorDirection.North:
				return DoorDirection.South;

			case DoorDirection.South:
				return DoorDirection.North;

			case DoorDirection.East:
				return DoorDirection.West;

			case DoorDirection.West:
				return DoorDirection.East; 

			default:
				throw new System.ArgumentOutOfRangeException(nameof(direction)); 
		}
	}

	private void CheckFirstConnection()
	{
		if (selectedRooms.Count < 2)
			return;

		connections = new RoomConnection[0]; 

		int firstIndex = selectedRooms[0];
		int secondIndex = selectedRooms[1];

		RoomDoor[] firstDoors = roomPrefabs[firstIndex].GetComponentsInChildren<RoomDoor>(true);

		RoomDoor[] secondDoors = roomPrefabs[secondIndex].GetComponentsInChildren<RoomDoor>(true);

		foreach (RoomDoor firstDoor in firstDoors)
		{
			DoorDirection neededDirection = GetOppositeDirection(firstDoor.Direction); 

			foreach (RoomDoor secondDoor in secondDoors)
			{
				if (secondDoor.Direction == neededDirection)
				{
					Debug.Log(
			                    "Compatible connection: " +
			                    roomPrefabs[firstIndex].name + " " +
			                    firstDoor.Direction + " <-> " +
			                    roomPrefabs[secondIndex].name + " " +
			                    secondDoor.Direction
				        );

					connections = new RoomConnection[]
					{
						new RoomConnection
						{
							roomAIndex = firstIndex,
							doorA = firstDoor.Direction,
							roomBIndex = secondIndex, 
							doorB = secondDoor.Direction
						}
					}; 

			                return;	
				}
			}
		}

		    Debug.LogWarning(
       			"No compatible doors between " +
		        roomPrefabs[firstIndex].name + " and " +
		        roomPrefabs[secondIndex].name
			);
	}

	public bool GenerateConnections()
	{
		List<RoomConnection> generatedConnections = new List<RoomConnection>(); 
		for (int i = 0; i < selectedRooms.Count - 1; i++)
		{
			bool connectionFound = false;
			int firstIndex = selectedRooms[i]; 
			int secondIndex = selectedRooms[i+1]; 
			Debug.Log("Checking " + 
					roomPrefabs[firstIndex].name + " -> " +
					roomPrefabs[secondIndex].name);

			RoomDoor[] firstDoors = roomPrefabs[firstIndex].GetComponentsInChildren<RoomDoor>(true);

			RoomDoor[] secondDoors = roomPrefabs[secondIndex].GetComponentsInChildren<RoomDoor>(true);

			ShuffleDoors(firstDoors);
			ShuffleDoors(secondDoors);

			foreach (RoomDoor firstDoor in firstDoors)
			{
				DoorDirection neededDirection = GetOppositeDirection(firstDoor.Direction); 
	
				foreach (RoomDoor secondDoor in secondDoors)
				{
					bool firstDoorUsed = IsDoorUsed(firstIndex, firstDoor.Direction, generatedConnections); 
					bool secondDoorUsed = IsDoorUsed(secondIndex, secondDoor.Direction, generatedConnections);

					if (secondDoor.Direction == neededDirection && !firstDoorUsed && !secondDoorUsed)
					{
						Debug.Log(
				                    "Compatible connection: " +
				                    roomPrefabs[firstIndex].name + " " +
				                    firstDoor.Direction + " <-> " +
				                    roomPrefabs[secondIndex].name + " " +
			        	            secondDoor.Direction
				        	);

						generatedConnections.Add(new RoomConnection 
						{
							roomAIndex = firstIndex,
							doorA = firstDoor.Direction,
							roomBIndex = secondIndex,
							doorB = secondDoor.Direction
						});

						connectionFound = true; 
						break; 
					}
				}

				if (connectionFound)
					break; 
			}
			
			if (!connectionFound)
			{
				Debug.LogWarning("Could not connect the selected rooms!");
				return false; 
			}
		}

		connections = generatedConnections.ToArray(); 

		foreach (RoomConnection connection in connections)
		{
		    Debug.Log(
		        roomPrefabs[connection.roomAIndex].name + " " +
		        connection.doorA + " <-> " +
		        roomPrefabs[connection.roomBIndex].name + " " +
		        connection.doorB
		    );
		}
		return true; 
	}

	private bool IsDoorUsed(int roomIndex, DoorDirection direction, List<RoomConnection> generatedConnections)
	{
		foreach (RoomConnection connection in generatedConnections)
		{
			if (connection.roomAIndex == roomIndex && connection.doorA == direction)
			{
				return true; 
			}

			if (connection.roomBIndex == roomIndex && connection.doorB == direction)
			{
				return true; 
			}
		}

		return false; 
	}

	private void ConfigureRoomDoors(GameObject room, int roomIndex)
	{
		RoomDoor[] doors = room.GetComponentsInChildren<RoomDoor>(true); 

		foreach (RoomDoor door in doors)
		{
			bool connected = false; 

			foreach (RoomConnection connection in connections)
			{
				if ((connection.roomAIndex == roomIndex && connection.doorA == door.Direction) || 
				(connection.roomBIndex == roomIndex && connection.doorB == door.Direction))
				{
					connected = true;
					break; 
				}
			}

			door.SetConnected(connected);
			Debug.Log(
   				"Room: " + roomPrefabs[roomIndex].name +
				" | Door: " + door.Direction +
    				" | Connected: " + connected
			);
		}
	}

	private void ShuffleDoors(RoomDoor[] doors)
	{
		for (int i = 0; i < doors.Length; i++)
		{
			int randomIndex = Random.Range(i, doors.Length); 

			RoomDoor temp = doors[i]; 
			doors[i] = doors[randomIndex]; 
			doors[randomIndex] = temp; 
		}
	}
}
