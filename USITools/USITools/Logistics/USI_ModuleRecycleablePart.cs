using System;
using System.Collections.Generic;

namespace USITools
{
    public class USI_ModuleRecycleablePart : PartModule
    {
        //Another super hacky module.
        private List<String> _blackList = new List<string> { "PotatoRoid", "UsiExplorationRock" };

        [KSPField]
        public float EVARange = 5f;

        [KSPField]
        public string ResourceName = "Recyclables";

        [KSPField]
        public string Menu = "Scrap Part";

        [KSPField]
        public float Efficiency = 0.8f;

        [KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiName = "Scrap part",
            unfocusedRange = 5f)]
        public void ScrapPart()
        {
            var kerbal = FlightGlobals.ActiveVessel.rootPart.protoModuleCrew[0];
            if (part.children.Count > 0)
            {
                ScreenMessages.PostScreenMessage("You can only scrap parts without child parts", 5f,
                    ScreenMessageStyle.UPPER_CENTER);
                return;
            }

            var res = PartResourceLibrary.Instance.GetDefinition(ResourceName);
            double resAmount = part.mass / res.density * Efficiency;

            if (!_blackList.Contains(part.partName))
            {
                ScreenMessages.PostScreenMessage(
                    String.Format("You disassemble the {0} into {1:0.00} units of {2}", part.name, resAmount,
                        ResourceName), 5f, ScreenMessageStyle.UPPER_CENTER);
                PushResources(ResourceName, resAmount);
            }
            part.decouple();
            part.explosionPotential = 0f;
            part.explode();
        }

        public override void OnStart(StartState state)
        {
            Events["ScrapPart"].unfocusedRange = EVARange;
            Events["ScrapPart"].guiName = Menu;
            base.OnStart(state);
        }

        private void PushResources(string resourceName, double amount)
        {
            var vessels = LogisticsTools.GetNearbyVessels(2000, true, vessel, false);
            var count = vessels.Count;
            for(int i = 0; i < count; ++i)
            {
                var v = vessels[i];
                //Put recycled stuff into recycleable places
                var mods = v.FindPartModulesImplementing<USI_ModuleRecycleBin>();
                var pCount = mods.Count;
                for (int x = 0; x < pCount; ++x)
                {
                    var p = mods[x].part;
                    if (p == part)
                        continue;

                    if (p.Resources.Contains(resourceName))
                    {
                        var partRes = p.Resources[resourceName];
                        var partNeed = partRes.maxAmount - partRes.amount;
                        if (partNeed > 0 && amount > 0)
                        {
                            if (partNeed > amount)
                            {
                                partNeed = amount;
                            }
                            partRes.amount += partNeed;
                            amount -= partNeed;
                        }
                    }
                }
            }
            if (amount > 1f)
            {
                ScreenMessages.PostScreenMessage(String.Format("{0:0} units of {1} were lost due to lack of recycle space", amount, ResourceName), 5f, ScreenMessageStyle.UPPER_CENTER);
            }
        }
    }

}