using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;

public class ArenaManager : MonoBehaviourPun
{
    public float spawn_time;
    public GameObject LobbyCamera;
    public GameObject joystick;
    public Transform[] spawns;
    public TMPro.TextMeshProUGUI spawn;
    float timer;
    bool has_player_spawned = false;
    // Start is called before the first frame update
    void Start()
    {
        LobbyCamera.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        spawn.text = "Spawning in " + Mathf.Round(spawn_time - timer);
        if (timer >= spawn_time)
        {
            if(!has_player_spawned)
            {
                Spawn();
            }
            timer = 0;
        }
    }

    public void Spawn()
    {

        Transform t_spawn = spawns[Random.Range(0, spawns.Length)];
        spawn.gameObject.SetActive(false);
        LobbyCamera.SetActive(false);
        joystick.SetActive(true);
        PhotonNetwork.Instantiate("Multi-player", t_spawn.position, t_spawn.rotation);
        has_player_spawned = true;
    }

}
