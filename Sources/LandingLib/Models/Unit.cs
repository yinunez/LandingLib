namespace LandingLib.Models
{
    internal class Unit
    {
        private UnitStatus status;
        public UnitStatus Status
        {
            get => LandingPlarformId == 0 ? UnitStatus.OutOfPlatform : status;
            set => status = value;
        }

        public int LandingPlarformId { get; set; }

        public string Rocket { get; set; }
    }
}
