﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour {

    private float startTime;
    private bool recording;
    private IList<Coords> replay;
    private bool onReplay;
    private int currentFrame;
    public GameObject prefab;
    private GameObject ghost;
    private GameObject player;
    
	public void Initialize () {
        player = GameObject.Find("Car");
        recording = true;
        startTime = Time.time;
        replay = new List<Coords>();
	}
	
	void Update () {
        if (recording)
        {
            float time = Time.time;
            GameObject.Find("Timer").GetComponent<UnityEngine.UI.Text>().text = ((int)((time - startTime) / 60)) + "min" + ((int)((time - startTime) % 60)) + "sec";
            //Debug.Log((int)((time - startTime) / 60) + "min" + (int)((time - startTime) % 60) + "sec");
            Coords c = new Coords();
            c.Position = player.transform.position;
            c.Rotation = player.transform.eulerAngles;
            replay.Add(c);
        }

        if (onReplay && currentFrame < replay.Count)
        {
            Coords tmp = replay[currentFrame];
            ghost.transform.position = tmp.Position;
            ghost.transform.eulerAngles = tmp.Rotation;
            currentFrame++;
        }
	}

    public void StopRecording()
    {
        recording = false;
    }

    public void Replay()
    {
        currentFrame = 0;
        if (!onReplay) ghost = Instantiate(prefab);
        onReplay = true;
        //ghost.GetComponentInChildren<Camera>().gameObject.SetActive(false);
    }

    public void Ghost()
    {
        Replay();
        player.transform.position = replay[0].Position;
        player.transform.eulerAngles = replay[0].Rotation;
        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        startTime = Time.time;
        GameObject.Find("Start(Clone)").GetComponent<Starting>().Tour = 1;
        GameObject.Find("Start(Clone)").GetComponent<Starting>().Started = false;
        GameObject.Find("CheckpointsManager").GetComponent<CheckpointManager>().TriggerStart();
    }

}
