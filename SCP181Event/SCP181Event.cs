using Smod2;
using Smod2.Attributes;
using Smod2.Events;
using Smod2.API;
using Smod2.EventHandlers;
using System.Collections.Generic;
using UnityEngine;

namespace SCP181
{
    [PluginDetails(
        author = "Squalalah",
        name = "SCP-181",
        description = "Un Classe-D aléatoire devient SCP-181 et défie les lois de la probabilité.",
        id = "squal.181.event",
        version = "1.0.1",
        SmodMajor = 3,
        SmodMinor = 1,
        SmodRevision = 12
        )]

    /////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////¸,ø¤º°`°º¤ø,¸¸,ø¤º° sqυαℓαℓαн °º¤ø,¸¸,ø¤º°`°º¤ø,¸//////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////SCP-181///////////////////////////////////////////
    /////////////////////////////////////////Description/////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////SCP-181 est une personne qui manipule les probabilités///////////////////
    //////////////////sans le vouloir, lui permettant d'avoir une chance inouie./////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////
    //////Dans ce plugin, un Classe-D aléatoire le devient, et dispose de pouvoirs comme...//////
    ////////////////////////////- Esquiver une attaque mortelle de SCP///////////////////////////
    ////////////////////////////- Ouvrir des portes sans la carte necessaire/////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////Au fur et à mesure des essais, la chance de SCP-181 diminue//////////////
    /////////////////////////////////////////////////////////////////////////////////////////////


    class SCP181Event : Plugin
    {
        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
            this.Info("SCP-181's event loaded successfully.");
        }

        public override void Register()
        {
            SCP181EventHandler eventh = new SCP181EventHandler(this);
            // Register with priority
            // On enregistre chaque EventHandler qu'on a utilisé.
            this.AddEventHandler(typeof(IEventHandlerRoundStart), eventh, Priority.Highest);
            this.AddEventHandler(typeof(IEventHandlerRoundEnd), eventh, Priority.Highest);
            this.AddEventHandler(typeof(IEventHandlerPlayerDropItem), eventh, Priority.Highest);
            this.AddEventHandler(typeof(IEventHandlerPlayerHurt), eventh, Priority.Highest);
            this.AddEventHandler(typeof(IEventHandlerPocketDimensionEnter), eventh, Priority.Highest);
            this.AddEventHandler(typeof(IEventHandlerPlayerDie), eventh, Priority.Highest);
            this.AddEventHandler(typeof(IEventHandlerDoorAccess), eventh, Priority.Highest);
            this.AddEventHandler(typeof(IEventHandlerSetRole), eventh, Priority.Highest);

        }
    }
}
