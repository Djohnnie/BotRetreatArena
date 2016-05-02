using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Custom.Scripts.Domain;
using UnityEngine.UI;

namespace com.terranovita.botretreat
{
    public class GUIController : MonoBehaviour
    {

        public Dropdown arenaDropdown;
        public Dropdown creatureDropdown;
        public Text topTeamsText;
        public SmoothFollow smoothFollow;


        // Update is called once per frame
        void Start()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            foreach( Image img in this.gameObject.GetComponentsInChildren<Image>()) {
                img.enabled = false;
            }
#endif

            //Adds a listener to the main slider and invokes a method when the value changes.
            creatureDropdown.onValueChanged.AddListener(delegate { creatureSelected(); });
            arenaDropdown.onValueChanged.AddListener(delegate { arenaSelected(); });
            InvokeRepeating("refreshArenas", 2, 10);
            InvokeRepeating("refreshCreatures", 2, 1);
            InvokeRepeating("refreshTopTeams", 2, 10);
        }
        public void creatureSelected()
        {
            smoothFollow.target = GridController.Instance.getCreatureByName(creatureDropdown.options[creatureDropdown.value].text);
        }
        public void arenaSelected()
        {
            GridController.Instance.selectArena(arenaDropdown.options[arenaDropdown.value].text);
            creatureDropdown.value = 0;
#if !UNITY_EDITOR && UNITY_WEBGL
            Application.ExternalCall( "selectArena", arenaDropdown.options[arenaDropdown.value].text );
#endif
            refreshTopTeams();
        }

        void refreshArenas()
        {
            Networking.Instance.refreshArenas(refreshArenasSuccessCallback, errorCallback);
        }

        void refreshCreatures()
        {
            creatureDropdown.ClearOptions();
            List<string> creatures = GridController.Instance.getCreatureNames();
            creatures.Insert(0, "All");
            creatureDropdown.AddOptions(creatures);
        }

        void refreshTopTeams()
        {
            if (arenaDropdown.value != null)
            {
                var arenaName = arenaDropdown.options[arenaDropdown.value].text;
                Networking.Instance.refreshTopTeams(arenaName, refreshTopTeamsSuccessCallback, refreshTopTeamsErrorCallback);
            }
        }

        public void selectArena(string name)
        {
            int count = 0;
            int toSelect = 0;
            foreach (Dropdown.OptionData data in arenaDropdown.options)
            {
                if (data.text == name)
                {
                    toSelect = count;
                    break;
                }
                count++;
            }
            arenaDropdown.value = toSelect;
            arenaSelected();
        }

        public void selectCreature(string name)
        {
            int count = 0;
            int toSelect = 0;
            foreach (Dropdown.OptionData data in creatureDropdown.options)
            {
                if (data.text == name)
                {
                    toSelect = count;
                    break;
                }
                count++;
            }
            creatureDropdown.value = toSelect;
            creatureSelected();
        }

        private void refreshArenasSuccessCallback(JSONObject json)
        {
            if (json.IsArray)
            {
                List<string> options = new List<string>();
                foreach (JSONObject name in json.list)
                {
                    options.Add(name.getStringValue("name"));
                }
                arenaDropdown.ClearOptions();
                arenaDropdown.AddOptions(options);
                if (!GridController.Instance.hasSelectedArena())
                {
                    arenaSelected();
                }

            }
        }

        private void refreshTopTeamsSuccessCallback(JSONObject json)
        {
            if (json.IsArray)
            {
                var topTeams = json.GetValues<TopTeam>();
                var topTeamsBuilder = new StringBuilder();
                topTeams.ForEach(x => topTeamsBuilder.Append(x.TeamName + " (" + x.NumberOfKills.ToString() + " kills, " + x.AverageBotLife + " abl)" + Environment.NewLine));
                topTeamsText.text = topTeamsBuilder.ToString();
            }
        }

        private void refreshTopTeamsErrorCallback(JSONObject json)
        {
            topTeamsText.text = String.Empty;
        }

        private void errorCallback(JSONObject json)
        {
            Debug.Log(json);
        }

    }
}
