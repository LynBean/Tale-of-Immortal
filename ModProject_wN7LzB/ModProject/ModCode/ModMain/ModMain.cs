using System;
using EGameTypeData;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_wN7LzB
{
    public class ModMain : MonoBehaviour
    {
        private Il2CppSystem.Action<ETypeData> callOpenUIPlayerInfo;

        public void Init()
        {
            this.callOpenUIPlayerInfo = new System.Action<ETypeData>(this.OnOpenUIPlayerInfo);
            g.events.On(EGameType.OpenUIEnd, this.callOpenUIPlayerInfo);
        }

        public void Destroy()
        {
            g.events.Off(EGameType.OpenUIEnd, this.callOpenUIPlayerInfo);
        }

        private void OnOpenUIPlayerInfo(ETypeData data)
        {
            OpenUIEnd openUIEnd = data.Cast<OpenUIEnd>();

            if (openUIEnd.uiType.uiName != UIType.PlayerInfo.uiName)
            {
                return;
            }

            UIPlayerInfo uiPlayerInfo = g.ui.GetUI<UIPlayerInfo>(UIType.PlayerInfo);
            if (uiPlayerInfo == null)
            {
                return;
            }

            UIPotmonList uIPotmonList = g.ui.CreateUI<UIPotmonList>(UIType.PotmonList);
            g.ui.CloseUI(uIPotmonList, false);

            Transform transformBtnFixName = uIPotmonList.transform.Find(
                "Group:Info/G:goUnit/attr/G:textName/G:btnFixName"
            );
            Transform transformGoFixName = uIPotmonList.transform.Find(
                "Group:Info/G:goUnit/attr/G:goFixName"
            );

            GameObject btnFixNameObject = UnityEngine.Object.Instantiate<GameObject>(
                transformBtnFixName.gameObject
            );
            GameObject goFixNameObject = UnityEngine.Object.Instantiate<GameObject>(
                transformGoFixName.gameObject
            );

            Transform transformPlayerInfoName = uiPlayerInfo.transform.Find(
                "Root/Group:Property/LanguageGroup/Item2/Item/G:goInfoRoot/Item/Text2"
            );
            GameObject playerInfoNameObject = transformPlayerInfoName.gameObject;
            Text playerInfoNameText = playerInfoNameObject.GetComponent<Text>();

            btnFixNameObject.transform.SetParent(transformPlayerInfoName, false);
            goFixNameObject.transform.SetParent(
                uiPlayerInfo.transform.Find(
                    "Root/Group:Property/LanguageGroup/Item2/Item/G:goInfoRoot/Item"
                ),
                false
            );

            Vector3 playerInfoNameTextVector = playerInfoNameText.transform.localPosition;
            btnFixNameObject.transform.localPosition = new Vector3(
                playerInfoNameTextVector.x + 110f,
                playerInfoNameTextVector.y,
                playerInfoNameTextVector.z
            );
            goFixNameObject.transform.localPosition = new Vector3(
                playerInfoNameTextVector.x - 170f,
                playerInfoNameTextVector.y - 40f,
                playerInfoNameTextVector.z
            );

            btnFixNameObject.SetActive(true);
            goFixNameObject.SetActive(false);

            Button btnFixNameButton = btnFixNameObject.GetComponent<Button>();

            Button goFixNameBtnOkButton = goFixNameObject
                .transform.Find("G:btnOk")
                .GetComponent<Button>();

            Button goFixNameBtnCancelButton = goFixNameObject
                .transform.Find("G:btnCancel")
                .GetComponent<Button>();

            InputField goFixNameIptNameInputField = goFixNameObject
                .transform.Find("LanguageGroup/G:iptName")
                .GetComponent<InputField>();

            Action btnFixNameButtonOnClick = delegate
            {
                btnFixNameObject.SetActive(false);
                goFixNameObject.SetActive(true);

                goFixNameIptNameInputField.text = "";
            };
            btnFixNameButton.onClick.AddListener(btnFixNameButtonOnClick);

            Action goFixNameBtnOkButtonOnClick = delegate
            {
                btnFixNameObject.SetActive(true);
                goFixNameObject.SetActive(false);

                DataUnit.PropertyData propertyData = g
                    .data.unit.GetUnit(g.data.world.playerUnitID)
                    .propertyData;

                string name = propertyData.GetName();
                string editedName = goFixNameIptNameInputField.text.Trim();

                if (string.IsNullOrEmpty(editedName) || name == editedName)
                {
                    return;
                }

                Il2CppStringArray array = new Il2CppStringArray(2);

                if (editedName.Contains(" "))
                {
                    string[] parts = editedName.Split(new[] { ' ' }, 2);
                    array[0] = parts[0];
                    array[1] = parts.Length > 1 ? parts[1] : "";
                }
                else
                {
                    array[0] = editedName.Length > 0 ? editedName.Substring(0, 1) : "";
                    array[1] = editedName.Length > 1 ? editedName.Substring(1) : "";
                }
                propertyData.name = array;
                playerInfoNameText.text = propertyData.GetName();
            };
            goFixNameBtnOkButton.onClick.AddListener(goFixNameBtnOkButtonOnClick);

            Action goFixNameBtnCancelButtonOnClick = delegate
            {
                btnFixNameObject.SetActive(true);
                goFixNameObject.SetActive(false);
            };
            goFixNameBtnCancelButton.onClick.AddListener(goFixNameBtnCancelButtonOnClick);
        }
    }
}
