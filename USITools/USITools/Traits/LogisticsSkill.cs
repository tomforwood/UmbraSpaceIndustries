using Experience;

namespace USITools
{
    public class LogisticsSkill : ExperienceEffect
    {
        public LogisticsSkill(ExperienceTrait parent) : base(parent)
        {
        }

        public LogisticsSkill(ExperienceTrait parent, float[] modifiers) : base(parent, modifiers)
        {
        }

        protected override float GetDefaultValue()
        {
            return 0f;
        }

        protected override string GetDescription()
        {
            return "Experience in managing logistics";
        }
    }
}