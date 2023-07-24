using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Pipes;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public NPS npc;

    bool isTalking = false;

    float distance;
    float curResponseTracker = 0;

    public GameObject player;
    public GameObject dialogueUI;

    public PlayerMovementGrappling pm;

    public Text NpsName;
    public Text NpsDialogueBox;
    public Text playerResponse;
    // Start is called before the first frame update
    void Start()
    {
        pm.GetComponent<PlayerMovementGrappling>().enabled = true;
        dialogueUI.SetActive(false);

    }
    private void OnMouseOver()
    {
        distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (distance <= 2.5f)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                curResponseTracker++;
                if (curResponseTracker >= npc.playerDialogue.Length - 1)
                {
                    curResponseTracker = npc.playerDialogue.Length - 1;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                curResponseTracker--;
                if (curResponseTracker < 0)
                {
                    curResponseTracker = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.E) && isTalking == false)
            {
                StartConversation();
            }
            else if (Input.GetKeyDown(KeyCode.E) && isTalking == true)
            {
                EndDialogue();
            }
            if (curResponseTracker == 0 && npc.playerDialogue.Length >= 0)
            {
                playerResponse.text = npc.playerDialogue[0];
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    NpsDialogueBox.text = npc.dialogue[1];
                }
            }
            else if (curResponseTracker == 1 && npc.playerDialogue.Length >= 1)
            {
                playerResponse.text = npc.playerDialogue[1];
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    NpsDialogueBox.text = npc.dialogue[2];
                }
            }

            void StartConversation()
            {
                isTalking = true;
                curResponseTracker = 0;
                dialogueUI.SetActive(true);
                pm.GetComponent<PlayerMovementGrappling>().enabled = false;
                NpsName.text = npc.name;
                NpsDialogueBox.text = npc.dialogue[0];
            }
            void EndDialogue()
            {
                pm.GetComponent<PlayerMovementGrappling>().enabled = true;
                isTalking = false;
                dialogueUI.SetActive(false);
            }
        }

    }
}