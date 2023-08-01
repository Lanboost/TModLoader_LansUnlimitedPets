using Terraria.ModLoader;
using LansUILib;
using Microsoft.Xna.Framework.Input;
using MonoMod.Cil;
using Terraria;

namespace LansUnlimitedPets
{
	public class LansUnlimitedPets : Mod
	{
        public override void Load()
        {
            Terraria.IL_Player.AddBuff_RemoveOldPetBuffsOfMatchingType += DisableRemovalOfPetsAndLightPets;
        }

        public void DisableRemovalOfPetsAndLightPets(ILContext iLContext)
        {
            LansUILib.InjectUtils.InjectSkipOnBoolean(iLContext, AlwaysSkip);
        }
        public static bool AlwaysSkip()
        {
            return true;
        }
    }
}