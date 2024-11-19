using System.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField] Connection inputConnection;

	public static ConnectionManager Instance { get; private set; }
	private void Awake()
	{
		// If there is an instance, and it's not me, delete myself.

		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	public enum Role
    {
        ServerClient = 0, Server = 1, Client = 2
    };

    private static Role _role = Role.ServerClient;

    void Start()
    {
        if (Application.isEditor)
        {
            _role = Role.ServerClient;
        }
        else if (Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
        {
			_role = Role.Client;
		}
		else if (Application.platform == RuntimePlatform.WindowsServer)
        {
			_role = Role.Server;
		}
    }

    public void Connect(Role role)
	{
		_role = role;

		CleanUpNonNetworkWorld();
		
		if (_role == Role.Server || _role == Role.ServerClient)
		{
			StartServerWorld();
		}
		
		if (_role == Role.Client || _role == Role.ServerClient) 
		{
			StartClientWorld();
        }
        
	}

	public World serverWorld;
	public World clientWorld;
	
    private void CleanUpNonNetworkWorld()
	{
		foreach (World world in World.All)
		{
			if (world.Flags == WorldFlags.Game)
			{
				world.Dispose();
				break;
			}
		}
	}

	private void StartServerWorld()
	{
		serverWorld = ClientServerBootstrap.CreateServerWorld("ServerWorld");
		
		NetworkEndpoint endpoint = NetworkEndpoint.AnyIpv4.WithPort(inputConnection.port);
		{
			using var query = serverWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
			query.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(endpoint);
		}
	}

	private void StartClientWorld()
	{
		clientWorld = ClientServerBootstrap.CreateClientWorld("ClientWorld");
		World.DefaultGameObjectInjectionWorld = clientWorld;
		
		SceneManager.LoadSceneAsync(1);
		
		NetworkEndpoint endpoint = NetworkEndpoint.Parse(inputConnection.ipAddress, inputConnection.port);
		{
			using var query = clientWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
			query.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(clientWorld.EntityManager, endpoint);
		}
	}
}
