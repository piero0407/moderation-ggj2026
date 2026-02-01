using System;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceController : MonoBehaviour
{
    [SerializeField] private GameObject fileTemplate;
    [SerializeField] private GameObject hierarchyMaster;
    [SerializeField] private Sprite[] fileTypes;

    private Button policeButton;

    public int evidenceAmmount { get; private set; }

    public void CallPolice()
    {
        GameManager.Instance.moderatorPerms = false;
        GameManager.Instance.ChangeState(GameManager.GameState.Win);
        Debug.Log("we should have won by now");
    }

    public void createNewEvidence(String name, Color asertionColor, int type)
    {
        GameObject go = Instantiate(fileTemplate, hierarchyMaster.transform);
        FileGenerator gen = go.GetComponent<FileGenerator>();

        gen.RefreshIcon(fileTypes[type != 911 ? type : 2], name, asertionColor);

        if (type == 911)
        {
            policeButton = go.GetComponent<Button>();
            if (policeButton != null)
            {
                policeButton.onClick.RemoveAllListeners();
                policeButton.onClick.AddListener(CallPolice);
                Debug.Log("POLICE.");
            }
        }

        evidenceAmmount += 1;
    }
}
