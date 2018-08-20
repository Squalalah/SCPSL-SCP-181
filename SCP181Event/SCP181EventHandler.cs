using Smod2;
using Smod2.Attributes;
using Smod2.Events;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Commands;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace SCP181
{
    class SCP181EventHandler : IEventHandlerRoundStart, IEventHandlerRoundEnd, IEventHandlerPlayerDropItem, IEventHandlerPlayerHurt, IEventHandlerPlayerDie, IEventHandlerDoorAccess, IEventHandlerSetRole, ICommandHandler
    {

        #region Vars

        private Plugin plugin;
        public Player Playerchosen; //L'élu devenant SCP-181
        private List<Player> players; //liste des joueurs au début du round, initialisé dans OnRoundStart
        private int max_tries = 5; //nombre de tentatives maximum pour l'esquive d'attaques de SCP
        private int max_door_tries = 5; //nombre de tentatives maximum d'ouverture de portes restreintes
        private int minimum_classe_d = 1;
        private int tries = 0; //nombre de tentatives d'esquive des attaques de SCP
        private int door_tries = 0; //nombre de tentatives d'ouvertures de portes restreintes
        private bool enabled = true;
        #endregion

        #region Class SCP181EventHandler

        public SCP181EventHandler(Plugin plugin)
        {
            this.plugin = plugin;
            players = new List<Player>();
        }

        #endregion

        #region OnRoundStart

        public void OnRoundStart(RoundStartEvent ev)
        {
            players = ev.Server.GetPlayers();
            enabled = plugin.GetConfigBool("enable_squal_scp_181");
            if(enabled)
            {
                max_tries = plugin.GetConfigInt("max_181_dodge_tries");
                max_door_tries = plugin.GetConfigInt("max_181_door_tries");
                minimum_classe_d = plugin.GetConfigInt("minimum_classe_d");

                List<Player> list = new List<Player>();
                if (players.Count == 0) plugin.Info("Not enough players to spawn SCP-181");
                else
                {
                    //S'il n'y a aucun joueur, on log le fait qu'il n'y a aucun classe D
                    foreach (Player p in players) //Pour chaque joueur stocké dans "players"
                    {
                        if (p.TeamRole.Role == Role.CLASSD) list.Add(p); //players.RemoveAt(players.IndexOf(p)); //Si son rôle n'est pas classe-D, on le retire de "players"
                    }
                    if (list.Count == 1) Playerchosen = list[0];
                    if (list.Count < minimum_classe_d) plugin.Info("Not enough ClassD to spawn SCP-181");
                    else
                    {
                        int index = UnityEngine.Random.Range(0, list.Count);
                        Playerchosen = list[index]; //"players" ne contenant que des classes D, nous tirons une personne au hasard entre 0 et le nombre de Classe-D en début de round.
                        plugin.Info(Playerchosen.Name + " becomes SCP-181.");
                        Playerchosen.GiveItem(ItemType.CUP); //On donne à l'élu un objet impossible à obtenir en jeu
                    }
                }
                list.Clear();
            }
            
        }

        #endregion

        #region OnRoundEnd

        public void OnRoundEnd(RoundEndEvent ev)
        {
            if (enabled)
            {
                players.Clear();
                Playerchosen = null;
                tries = 0;
                door_tries = 0;
            }      
        }

        #endregion

        #region OnPlayerDropItem

        public void OnPlayerDropItem(PlayerDropItemEvent ev)
        {
            //plugin.Info(ev.Player.Name + "jette un objet au sol");
            //if (ev && ev.Player.TeamRole.Role == Role.CLASSD) ev.Player.GiveItem(ItemType.CUP); //Etant donné que le gobelet permet de savoir si un joueur est SCP ou non, on redonnera l'objet au joueur si celui-ci veut le retirer de son inventaire
        }

        #endregion

        #region OnPlayerHurt

        public void OnPlayerHurt(PlayerHurtEvent ev)
        {
            if (enabled)
            {
                //plugin.Info(ev.Attacker.Name + " attacked " + ev.Player.Name);
                //plugin.Info("Damage received : " + ev.Damage);
                //plugin.Info("DamageType : " + ev.DamageType);
                if (Playerchosen.SteamId == ev.Player.SteamId)
                {
                    //plugin.Info("Le joueur has a CUP");
                    //plugin.Info("teamrole attacker = " + ev.Attacker.TeamRole.Name);
                    //plugin.Info("team attacker = " + ev.Attacker.TeamRole.Team);
                    if (ev.DamageType != DamageType.POCKET && ev.Damage != 1)//If the player is in the pocket dimension, his luck doesn't work.
                    {
                        if (ev.Attacker.TeamRole.Team == Smod2.API.Team.SCP || ev.DamageType == DamageType.SCP_096) //If the attacker is a SCP
                        {
                            if (max_tries != 0)
                            {
                                //On calcule une chance aléatoire afin de savoir si SCP-181 va esquiver le coup ou pas
                                if (UnityEngine.Random.Range(0, max_tries) > tries) //S'il y arrive     
                                {
                                    ev.Damage = 0;
                                    //plugin.Info(Playerchosen.Name + " successfully dodged scp's attack.");
                                }
                                tries++;
                                //plugin.Info(Playerchosen.Name + " only have " + (max_tries - tries) + " chances on " + max_tries + " to dodge scp's attacks.");
                            }
                        }
                    }
                    
                }
            }
        }

        #endregion

        #region OnPlayerDie

        public void OnPlayerDie(PlayerDeathEvent ev)
        {
            if (enabled)
            {
                if (ev.Player.SteamId == Playerchosen.SteamId) Playerchosen = null;
            }
        }

        #endregion

        #region OnDoorAccess

        public void OnDoorAccess(PlayerDoorAccessEvent ev)
        {
            if (enabled)
            {
                if (Playerchosen.SteamId == ev.Player.SteamId)
                {
                    try
                    {
                        Smod2.API.Item itemhandle = ev.Player.GetCurrentItem();
                        //plugin.Info("Object held by the player : " + itemhandle);
                    }
                    catch
                    {
                        if (ev.Door.Permission.Length > 0) //If the door has permissions (restricted card)
                        {
                            if(max_door_tries != 0)
                            {
                                if (UnityEngine.Random.Range(0, max_door_tries + 1) > door_tries)
                                {
                                    ev.Allow = true;
                                }
                                door_tries++;
                            }
                            
                            //plugin.Info(ev.Player.Name + " only have " + (max_door_tries - door_tries) + " chances on " + max_door_tries + " to open restricted doors without card");
                        }
                    }
                }
            }
        }
        #endregion

        #region OnSetRole

        public void OnSetRole(PlayerSetRoleEvent ev)
        {
            if (ev.Player.HasItem(ItemType.CUP) && ev.Player.SteamId == Playerchosen.SteamId) ev.Player.GetInventory().RemoveAt(ev.Player.GetItemIndex(ItemType.CUP));
        }
        #endregion

        #region Commands

        public string GetCommandDescription()
        {
            // This prints when someone types HELP HELLO
            return "Knows who play SCP-181";
        }

        public string GetUsage()
        {
            // This prints when someone types HELP HELLO
            return "SCP181";
        }

        public string[] OnCall(ICommandSender sender, string[] args)
        {
            // This will print 3 lines in console.
            try
            {
                if (enabled)
                {
                    return new string[] { "Who plays SCP-181 ?", Playerchosen.Name + " plays SCP-181 (" + Playerchosen.SteamId + ")" };
                }
                else return new string[] { "Who plays SCP-181 ?", "Plugin disabled." };
            }
            catch
            {
                return new string[] { "Who plays SCP-181 ?", "Nobody's playing SCP-181" };
            }
        }
        #endregion
    }

}
