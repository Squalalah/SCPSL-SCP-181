using Smod2;
using Smod2.Attributes;
using Smod2.Events;
using Smod2.API;
using Smod2.EventHandlers;
using System.Collections.Generic;
using UnityEngine;

namespace SCP181
{
    class SCP181EventHandler : IEventHandlerRoundStart, IEventHandlerRoundEnd, IEventHandlerPlayerDropItem, IEventHandlerPlayerHurt, IEventHandlerPocketDimensionEnter, IEventHandlerPlayerDie, IEventHandlerDoorAccess, IEventHandlerSetRole
    {
        private Plugin plugin;
        private List<Player> players; //liste des joueurs au début du round, initialisé dans OnRoundStart
        private int max_tries = 5; //nombre de tentatives maximum pour l'esquive d'attaques de SCP
        private int max_door_tries = 5; //nombre de tentatives maximum d'ouverture de portes restreintes
        private int minimum_classe_d = 1;
        private int tries = 0; //nombre de tentatives d'esquive des attaques de SCP
        private int door_tries = 0; //nombre de tentatives d'ouvertures de portes restreintes
        public Player Playerchosen; //L'élu devenant SCP-181

        public SCP181EventHandler(Plugin plugin)
        {
            this.plugin = plugin;

            plugin.Info("SCP181EventHandler créer");
            if (!System.IO.File.Exists("181.txt"))
            {
                string text = "#max_181_dodge_tries:" + max_tries + System.Environment.NewLine
                    + "#max_181_door_tries:" + max_door_tries + System.Environment.NewLine
                    + "#minimum_classe_d:" + minimum_classe_d;
                System.IO.File.WriteAllText("181.txt", text);
            }
            else
            {

                string[] content = System.IO.File.ReadAllLines("181.txt");
                string[] value;
                if (content.Length != 3) Rewritefile();
                else
                {
                    foreach (string s in content)
                    {
                        value = s.Split(':');
                        try
                        {
                            if (value[0] == "#max_181_dodge_tries") max_tries = int.Parse(value[1]);
                            else if (value[0] == "#max_181_door_tries") max_door_tries = int.Parse(value[1]);
                            else if (value[0] == "#minimum_classe_d") minimum_classe_d = int.Parse(value[1]);
                            else Rewritefile();
                        }
                        catch { Rewritefile(); }
                    }
                }
                plugin.Info("Le nombre de tentative d'esquive de SCP-181 est fixé à : " + max_tries);
                plugin.Info("Le nombre de tentative d'ouverture de portes de SCP-181 est fixé à : " + max_door_tries);
            }
        }

        public void Rewritefile()
        {
            string text = "#max_181_dodge_tries:" + max_tries + System.Environment.NewLine
                    + "#max_181_door_tries:" + max_door_tries + System.Environment.NewLine
                    + "#minimum_classe_d:" + minimum_classe_d;
            System.IO.File.WriteAllText("181.txt", text);
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            plugin.Info("On recupere tout les joueurs)");
            players = plugin.pluginManager.Server.GetPlayers(); //On recupère tout les joueurs au démarrage du Round (juste avant le spawn)
            plugin.Info("On regarde s'il y a des classe-D)");
            List<Player> list = new List<Player>();
            int lol;
            if (players.Count == 0) plugin.Info("Pas assez de Classe-D pour devenir SCP-181");
            else
            {
                //S'il n'y a aucun joueur, on log le fait qu'il n'y a aucun classe D
                plugin.Info("Pour chaque joueur)");
                foreach (Player p in players) //Pour chaque joueur stocké dans "players"
                {
                    plugin.Info("Si le joueur n'est pas un classe-D, on le retire de la liste");
                    if (p.TeamRole.Role == Role.CLASSD) list.Add(p);//players.RemoveAt(players.IndexOf(p)); //Si son rôle n'est pas classe-D, on le retire de "players"
                }
                if (list.Count == 1) Playerchosen = list[0];
                else if (list.Count < minimum_classe_d) plugin.Info("Pas assez de classe D pour faire spawn SCP-181-");
                else
                {
                    lol = Random.Range(0, list.Count);
                    Playerchosen = list[lol]; //"players" ne contenant que des classes D, nous tirons une personne au hasard entre 0 et le nombre de Classe-D en début de round.
                    plugin.Info(Playerchosen.Name + " devient SCP-181.");
                    Playerchosen.GiveItem(ItemType.CUP); //On donne à l'élu un objet impossible à obtenir en jeu
                    Playerchosen.SendConsoleMessage("Tu es SCP 181."); //On log le fait qu'il le devienne
                }               
            }
        }

        public void OnRoundEnd(RoundEndEvent ev)
        {
            //A la fin du round, on supprime toutes les informations sur le SCP
            players.Clear();
            Playerchosen = null;
            tries = 0;
        }

        public void OnPlayerDropItem(PlayerDropItemEvent ev)
        {
            plugin.Info(ev.Player.Name + "jette un objet au sol");
            if (ev.Item.ItemType == ItemType.CUP && ev.Player.TeamRole.Role == Role.CLASSD) ev.Player.GiveItem(ItemType.CUP); //Etant donné que le gobelet permet de savoir si un joueur est SCP ou non, on redonnera l'objet au joueur si celui-ci veut le retirer de son inventaire
        }

        public void OnPlayerHurt(PlayerHurtEvent ev)
        {
            //plugin.Info(ev.Attacker.Name + " a tenté d'attaquer " + ev.Player.Name);
            //plugin.Info("Damage recieved : " + ev.Damage);
            //plugin.Info("DamageType : " + ev.DamageType);
            if (ev.Player.HasItem(ItemType.CUP))
            {
                //plugin.Info("Le joueur a une CUP");
                //plugin.Info("teamrole attacker = " + ev.Attacker.TeamRole.Name);
                //plugin.Info("team attacker = " + ev.Attacker.TeamRole.Team);
                if (ev.DamageType == DamageType.POCKET && ev.Damage == 1) return;
                if (ev.Attacker.TeamRole.Team == Smod2.API.Team.SCP || ev.DamageType == DamageType.SCP_096) //Si la personne ayant touché Player est un SCP
                {
                    //plugin.Info("L'attaquant est un SCP");
                    //On calcule une chance aléatoire afin de savoir si SCP-181 va esquiver le coup ou pas
                    if (Random.Range(0, max_tries) > tries) //S'il y arrive     
                    {
                        ev.Player.AddHealth((int)ev.Damage);
                        //plugin.Info(Playerchosen.Name + " a reussit à esquiver de justesse une attaque de SCP");
                    }
                    tries++;
                    //plugin.Info(Playerchosen.Name + " n'a plus que " + (max_tries - tries) + " chances sur " + max_tries + " d'esquiver les coups des SCP");
                }
            }
        }

        public void OnPocketDimensionEnter(PlayerPocketDimensionEnterEvent ev)
        {
            /* plugin.Info("Le joueur entre dans la dimension");
             if(ev.Player.HasItem(ItemType.CUP))
             {
                 plugin.Info("Le joueur a la CUP");
                 //Quand SCP-181 entre dans la dimension, on génère une chance qu'il retourne à l'endroit où il était originellement    
                 if (Random.Range(0, max_tries) > tries) //S'il a de la chance, on le téléporte.
                 {
                     ev.Damage = 0;
                     ev.Player.Teleport(ev.LastPosition);
                     plugin.Info("Le joueur a de la chance et arrive à sortir");
                 }
                 tries++;
                 plugin.Info(ev.Player.Name + " n'a plus que " + (max_door_tries - door_tries) + " chances sur " + max_door_tries + " d'ouvrir des portes vérouillés sans carte");
             }*/
        }

        public void OnPlayerDie(PlayerDeathEvent ev)
        {
        }

        public void OnDoorAccess(PlayerDoorAccessEvent ev)
        {
            if (ev.Player.HasItem(ItemType.CUP))
            {
                try
                {
                    Smod2.API.Item itemhandle = ev.Player.GetCurrentItem();
                    //plugin.Info("Objet tenu par le D-boy : " + itemhandle);
                }
                catch
                {
                    if (ev.Door.Permission.Length > 0) //Si la porte en question a des permissions (carte obligatoire)
                    {
                        if (Random.Range(0, max_door_tries + 1) > door_tries) //On génère une chance aléatoire d'ouvrir la porte sans la carte necessaire
                        {
                            ev.Allow = true;
                            plugin.Info(ev.Player.Name + " a reussit à ouvrir une porte sans la carte adéquate.");
                        }
                        door_tries++;
                        //plugin.Info(ev.Player.Name + " n'a plus que " + (max_door_tries - door_tries) + " chances sur " + max_door_tries + " d'ouvrir des portes vérouillés sans carte");
                    }
                }
            }
        }

        public void OnSetRole(PlayerSetRoleEvent ev)
        {
            if (ev.Player.HasItem(ItemType.CUP) && ev.Role != Role.CLASSD) ev.Player.GetInventory().RemoveAt(ev.Player.GetItemIndex(ItemType.CUP));
        }

    }

}
