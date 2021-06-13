namespace ResourceTypes.Navigation
{
    public static class AIWorld_Factory
    {
        public static IType ConstructByTypeID(ushort ID)
        {
            IType Output = null;

            switch (ID)
            {
                case 1:
                    Output = new AIWorld_Type1();
                    break;
                case 2:
                    //Output = new AIWorld_Type2();
                    break;
                case 4:
                    Output = new AIWorld_Type4();
                    break;
                case 7:
                    Output = new AIWorld_Type7();
                    break;
                case 8:
                    Output = new AIWorld_Type8();
                    break;
                case 9:
                    Output = new AIWorld_Type8();
                    break;
                case 11:
                    Output = new AIWorld_Type11();
                    break;
                default:
                    break;
            }

            return Output;
        }

        public static ushort GetIDByType(IType Object)
        {
            if (Object is AIWorld_Type1)
            {
                return 1;
            }
            else if (Object is AIWorld_Type11)
            {
                return 11;
            }
            else if (Object is AIWorld_Type4)
            {
                return 4;
            }
            else if (Object is AIWorld_Type7)
            {
                return 7;
            }
            else if (Object is AIWorld_Type8)
            {
                return 8;
            }
            else
            {
                return ushort.MaxValue;
            }
        }
    }
}
