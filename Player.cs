using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LansUILib;
using LansUILib.ui;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace LansUnlimitedPets
{
    public class PetPlayer: ModPlayer
    {
        bool showingUI = false;

        LansUILib.ui.LComponent panel;
        LansUILib.ui.PanelSettings panelSettings = new PanelSettings();

        List<LansUILib.ui.LItemSlot> itemSlots = new List<LItemSlot>();
        LItemSlot emptySlot = new LItemSlot(LItemSlotType.PetAndLight);

        public override void Initialize()
        {
            base.Initialize();
            panelSettings.SetAnchor(LansUILib.ui.AnchorPosition.Center);
            panelSettings.SetSize(-150, -150, 300, 300);
        }

        protected void createPanel()
        {
            panel = LansUILib.UIFactory.CreatePanel("Unlimited Pets Main", panelSettings, true, false);

            var inner = new LComponent("Inner");
            inner.isMask = true;
            panel.Add(inner);

            inner.SetMargins(15, 15, 15, 15);
            inner.SetLayout(new LansUILib.ui.LayoutFlow(new bool[] { false, false }, new bool[] { true, true }, LayoutFlowType.Vertical, 0, 0, 0, 0, 10));


            inner.Add(LansUILib.UIFactory.CreateText("Pets panel (Show with pet ui)", true));

            var scrollpanel = UIFactory.CreateScrollPanel();
            scrollpanel.wrapper.GetLayout().Flex = 1;

            //var recipePanel = LansUILib.UIFactory.CreatePanel("Recipe Panel", false, false);
            var scrollContentPanel = scrollpanel.contentPanel;
            scrollContentPanel.SetLayout(new LansUILib.ui.LayoutGrid(6, new bool[] { true, true},new bool[] { false, false },LayoutGridType.Columns, 10, 0, 0, 0, 5));
            inner.Add(scrollpanel.wrapper);


            foreach (var slot in itemSlots)
            {
                var itemSlotPanel = LansUILib.UIFactory.CreateItemSlot(slot);
                scrollContentPanel.Add(itemSlotPanel);
            }

            {
                var itemSlotPanel = LansUILib.UIFactory.CreateItemSlot(emptySlot);
                scrollContentPanel.Add(itemSlotPanel);
            }
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            LansUILib.ui.LItemSlot[] tmpItemSlots = new LItemSlot[0];
            LansUILib.ui.LItemSlot.Load(tag, "itemData", ref tmpItemSlots, LItemSlotType.PetAndLight);
            itemSlots.AddRange(tmpItemSlots);
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            LansUILib.ui.LItemSlot.Save(tag, "itemData", itemSlots.ToArray());
            if (panel != null)
            {
                LansUILib.UISystem.Instance.Screen.Remove(panel);
                panel = null;
            }
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

            bool needRefresh = false;
            for (int i = itemSlots.Count-1; i >= 0; i--)
            {
                if (itemSlots[i].Item.IsAir)
                {
                    itemSlots.RemoveAt(i);
                    needRefresh = true;
                }
            }
            if(!emptySlot.Item.IsAir)
            {
                itemSlots.Add(emptySlot);
                emptySlot = new LItemSlot(LItemSlotType.PetAndLight);
                needRefresh = true;
            }

            if(needRefresh)
            {
                if (showingUI)
                {
                    LansUILib.UISystem.Instance.Screen.Remove(panel);
                    panel = null;
                    createPanel();
                    LansUILib.UISystem.Instance.Screen.Add(panel);
                    panel.Invalidate();
                }

            }

            if (newState != showingUI)
            {
                showingUI = newState;
                if(showingUI)
                {
                    if(panel != null)
                    {
                        LansUILib.UISystem.Instance.Screen.Remove(panel);
                        panel = null;
                    }

                    createPanel();
                    LansUILib.UISystem.Instance.Screen.Add(panel);
                    panel.Invalidate();
                }
                else
                {
                    LansUILib.UISystem.Instance.Screen.Remove(panel);
                    panel = null;
                }
            }


            foreach (var slot in itemSlots)
            {
                slot.Update();
            }
        }
    }
}
