using UnityEngine;

public enum DoorDirection 
{
	North,
	South,
	East,
	West
}

public class RoomDoor : MonoBehaviour
{
	[SerializeField] private DoorDirection direction;
	[SerializeField] private Transform spawnPoint;
	[SerializeField] private GameObject doorBlocker;

	public DoorDirection Direction => direction; 
	public Transform SpawnPoint => spawnPoint;
	private bool isConnected;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Player"))
			return; 

		if (RoomManager.Instance != null || !isConnected) 
		{
			RoomManager.Instance.TryGoThroughDoor(direction); 
		}
	}

	public void SetConnected(bool connected)
	{
		isConnected = connected;
		if (doorBlocker != null)
		{
			doorBlocker.SetActive(!connected); 
		}
	}
}
