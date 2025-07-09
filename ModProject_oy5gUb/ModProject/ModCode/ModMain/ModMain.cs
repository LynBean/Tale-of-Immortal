namespace MOD_oy5gUb
{
    public class ModMain
    {
        private Il2CppSystem.Action<ETypeData> callInitTrigger;

        public void Init()
        {
            this.callInitTrigger = (Il2CppSystem.Action<ETypeData>)InitTrigger;
            g.events.On(EGameType.IntoWorld, this.callInitTrigger);
        }
        public void Destroy()
        {
            g.events.Off(EGameType.IntoWorld, this.callInitTrigger);
        }


        private void InitTrigger(ETypeData data)
        {
            DataProps propData = MapBuildTownStorage.GetPlayerStorage().data.propData;
            propData.gridMaxNum = int.MaxValue;
        }

    }
}
