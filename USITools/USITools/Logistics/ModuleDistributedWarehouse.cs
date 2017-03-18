//using System;
//using System.Collections.Generic;
//using USITools.Logistics;

//namespace USITools
//{
//    public class ModuleLogisticsCenter : PartModule
//    {
//    }


//    [KSPModule("Distributed Warehouse")]
//    public class ModuleDistributedWarehouse : PartModule
//    {
//        [KSPField] 
//        public float LogisticsRange = 2000f;

//        private double lastCheck;

//        public void FixedUpdate()
//        {
//            if (!HighLogic.LoadedSceneIsFlight)
//                return;

//            if (Math.Abs(Planetarium.GetUniversalTime() - lastCheck) < LogisticsSetup.Instance.Config.WarehouseTime)
//                return;

//            lastCheck = Planetarium.GetUniversalTime();

//            var rCount = part.Resources.Count;
//            for (int i = 0; i < rCount; ++i)
//            {
//                var res = part.Resources[i];

//                LevelResources(res.resourceName);
//            }
//        }

//        private void LevelResources(string resource)
//        {
//            var nearbyShips = LogisticsTools.GetNearbyVessels(LogisticsRange, true, vessel, true);
//            var depots = new List<Vessel>();
//            for_each (var d in nearbyShips)
//            {
//                if (d.FindPartModulesImplementing<ModuleDistributedWarehouse>().Any()
//                    && d.Parts.Any(p=>p.Resources.Contains(resource)))
//                    depots.Add(d);
//            }
//            //Get relevant parts
//            var resParts = new List<Part>();
//            for_each (var d in depots)
//            {
//                var pList = d.parts.Where(p => p.Resources.Contains(resource) && p.Modules.Contains("ModuleDistributedWarehouse"));
//                for_each (var p in pList)
//                {
//                    var wh = p.FindModuleImplementing<USI_ModuleResourceWarehouse>();
//                    if(wh != null && wh.transferEnabled)
//                        resParts.Add(p);
//                }
//            }
//            var amountSum = 0d;
//            var maxSum = 0d;

//            //Figure out our average fill percent
//            for_each (var p in resParts)
//            {
//                var res = p.Resources[resource];
//                amountSum += res.amount;
//                maxSum += res.maxAmount;
//            }
//            if (maxSum > 0 && amountSum > 0)
//            {
//                double fillPercent = amountSum/maxSum;
//                //Level everything
//                for_each (var p in resParts)
//                {
//                    var res = p.Resources[resource];
//                    res.amount = res.maxAmount*fillPercent;
//                }
//            }
//        }

//        // Info about the module in the Editor part list
//        public override string GetInfo()
//        {
//            return "Balances nearby warehouse levels\n\n" +
//                "LogisticsRange: " + LogisticsRange + "m";
//        }
//    }
//}