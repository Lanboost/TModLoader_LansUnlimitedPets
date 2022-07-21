using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LansUILib.ui;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace LansUnlimitedPets
{
    public class PetPlayer: ModPlayer
    {
        bool showingUI = false;

        LansUILib.ui.WrapperComponent panel;

        LansUILib.ui.LItemSlot[] itemSlots = LansUILib.ui.LItemSlot.Create(10);


        public override void Initialize()
        {
            base.Initialize();

            if (panel != null)
            {
                showingUI = false;
                LansUILib.UISystem.Instance.Screen.Remove(panel);
            }

            panel = LansUILib.UIFactory.CreateUIPanel("Unlimited Pets Main");

            panel.SetAnchor(LansUILib.ui.AnchorPosition.Center);
            panel.SetSize(-50, -50, 100, 100);

            panel.SetLayout(new LansUILib.ui.LayoutFlow(new bool[] { true, true }, new bool[] { false, false }, LayoutFlowType.Vertical, 0, 0, 24, 24, 10));

            panel.Add(LansUILib.UIFactory.CreateText("Text", "Mod is in WIP (Show ui by accessing normal pet slot)"));

            var slotPanel = LansUILib.UIFactory.CreatePanel("Unlimited Pets Main");
            slotPanel.SetLayout(new LansUILib.ui.LayoutFlow(new bool[] { true, true }, new bool[] { false, false }, LayoutFlowType.Horizontal, 0, 0, 0, 0, 5));
            panel.Add(slotPanel);
            foreach (var slot in itemSlots)
            {
                var itemSlotPanel = LansUILib.UIFactory.CreateItemSlot(slot);
                slotPanel.Add(itemSlotPanel);
            }

        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            LansUILib.ui.LItemSlot.Load(tag, "itemData", ref itemSlots);
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            LansUILib.ui.LItemSlot.Save(tag, "itemData", itemSlots);
        }

        public override void PreUpdateBuffs()
        {
            base.PreUpdateBuffs();
            var newState = showingUI;
            if (Main.EquipPage == 2)
            {
                newState = true;
            }
            else { 
                newState = false; 
            }

            if(newState != showingUI)
            {
                showingUI = newState;
                if(showingUI)
                {
                    LansUILib.UISystem.Instance.Screen.Add(panel);
                    panel.Invalidate();
                }
                else
                {
                    LansUILib.UISystem.Instance.Screen.Remove(panel);
                }
            }


            foreach (var slot in itemSlots)
            {
                slot.Update();
            }

        }
    }
}
