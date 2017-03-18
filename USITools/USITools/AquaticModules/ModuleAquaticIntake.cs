﻿namespace USITools
{
    public class ModuleAquaticIntake : PartModule
    {
        [KSPField]
        public string ResourceName = "";

        [KSPField] 
        public float FlowRate = 1.0f;

        public void FixedUpdate()
        {
            if (!HighLogic.LoadedSceneIsFlight)
                return;
            vessel.checkSplashed();
            if (!vessel.Splashed)
                return;
            var res = part.Resources.Get(ResourceName);
            if (res != null)
            {
                res.amount += (FlowRate*TimeWarp.fixedDeltaTime);
                if(res.amount > res.maxAmount)
                    res.amount = res.maxAmount;
            }
        }
    }
}