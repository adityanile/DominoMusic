using System.Collections.Generic;
using UnityEngine;

public class PianoNotes : MonoBehaviour
{
    AudioClip[] allTunes;

    public int startFromOctave = 1;

    public string[] alpha;
    public List<AudioClip> order = new List<AudioClip>();

    public SpawnAlongTheLine spawn;

    // Start is called before the first frame update
    void Start()
    {
        allTunes = Resources.LoadAll<AudioClip>("pianoNotes/");

        for (int i = startFromOctave; i < 8; i++)
        {   
            for (int j = 0; j < alpha.Length; j++)
            {
                string str = alpha[j] + i.ToString();
                order.Add( Find(str) );
            }
        }
        spawn.tunes = order;
    }

    AudioClip Find(string name)
    {
        int index = -1;

        for (int i = 0; i < allTunes.Length; i++)
        {
            if (allTunes[i].name == name)
            {
                index = i;
            }
        }

        if (index == -1)
            return null;
        else
            return allTunes[index];
    }
}

