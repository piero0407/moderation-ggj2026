using System;
using System.IO;
using System.Runtime.CompilerServices;
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
    }

    public void createNewEvidence(String name, Color asertionColor, int type)
    {
        GameObject.Instantiate(fileTemplate, hierarchyMaster.transform);
        FileGenerator gen = fileTemplate.GetComponent<FileGenerator>();

        gen.RefreshIcon(fileTypes[type != 911 ? type : 2], name, asertionColor);

        if (type == 911)
        {
            policeButton = gen.gameObject.GetComponent<Button>();
            if (policeButton != null) policeButton.onClick.AddListener(CallPolice);
        }

        evidenceAmmount += 1;
    }
}
