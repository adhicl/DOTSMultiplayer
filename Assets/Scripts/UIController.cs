using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
	private VisualElement windowJoin;

	private Button btnRandom;
	private Button btnCreate;
	private Button btnJoin;

	private TextField txtIpAddress;
	private TextField txtPort;

	// Use this for initialization
	void Start()
	{
		VisualElement root = GetComponent<UIDocument>().rootVisualElement;
		windowJoin = root.Q<VisualElement>("JoinWindow");
		btnRandom = root.Q<Button>("JoinBtn");
		btnCreate = root.Q<Button>("CreateRoomBtn");
		btnJoin = root.Q<Button>("JoinRoomBtn");

		txtIpAddress = root.Q<TextField>("AddressTxt");
		txtPort = root.Q<TextField>("PortTxt");

		txtIpAddress.value = "127.00.00.00";
		txtPort.value = "8080";

		//btnCreate.SetEnabled(false);
		//btnJoin.SetEnabled(false);

		btnRandom.RegisterCallback<ClickEvent>(OnJoinRandom);
		btnCreate.RegisterCallback<ClickEvent>(OnCreateRoomRandom);
		btnJoin.RegisterCallback<ClickEvent>(OnJoinRoomRandom);
		
	}

	private void OnJoinRandom(ClickEvent ev)
	{
		//windowJoin.style.display = DisplayStyle.None;
		windowJoin.AddToClassList("WindowDown");

		ConnectionManager.Instance.Connect(ConnectionManager.Role.ServerClient);
	}

	private void OnCreateRoomRandom(ClickEvent ev)
	{
		//windowJoin.style.display = DisplayStyle.None;
		windowJoin.AddToClassList("WindowDown");
		ConnectionManager.Instance.Connect(ConnectionManager.Role.ServerClient);
	}

	private void OnJoinRoomRandom(ClickEvent ev)
	{
		//windowJoin.style.display = DisplayStyle.None;
		windowJoin.AddToClassList("WindowDown");
		ConnectionManager.Instance.Connect(ConnectionManager.Role.Client);
	}

}