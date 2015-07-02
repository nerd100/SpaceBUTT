using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceBUTT
{
    class Menu
    {
        private static int maxItems = 20;   //Gibt die maximale Anzahl der Menüeinträge an
        private int menuItemCount;          //Gibt die aktuelle Anzahl aller Menüeinträge an
        private int curMenuItem;            //Gibt den aktuellen Menüeintragindex an
        private string[] menuItems;         //Array mit allen Menünamen
        private Vector2[] pos;              //Array mit allen Menüpositionen
        private double[] scale;             //Array mit allen Skalierungen der Menüeinträge
        private Color unselected;           //Farbe der aktuell nicht ausgewählten Einträge
        private Color selected;             //Farbe des ausgewählten Eintrages
        private SpriteFont font;            //Fontdatei des Menüs

        //Constructor des Menüs
        //Initialisiert Standardwerte
        public Menu(Color unselectedColor, Color selectedColor, SpriteFont font)
        {
            this.font = font;
            this.menuItems = new string[maxItems];
            this.pos = new Vector2[maxItems];
            this.scale = new double[maxItems];
            this.unselected = unselectedColor;
            this.selected = selectedColor;
            this.menuItemCount = 0;
            this.curMenuItem = 0;
        }

        //Methode zum Erstellen eines neuen Menüeintrages
        //Übergibt den Namen und Position
        public void AddMenuItem(string name, Vector2 p)
        {
            //Prüft ob bereits die maximale Anzahl erreicht wurde
            if (menuItemCount < maxItems)
            {
                menuItems[menuItemCount] = name;
                scale[menuItemCount] = 1.0f;
                pos[menuItemCount++] = p;
            }
        }

        //Methode um den nächsten Menüpunkt auszuwählen
        //Nach letztem Menüpunkt zum ersten zurückspringen
        public void SelectNext()
        {
            if (curMenuItem < menuItemCount - 1)
            {
                curMenuItem++;
            }
            else
            {
                curMenuItem = 0;
            }
        }

        //Methode um den letzten Menüpunkt auszuwählen
        //Nach erstem Menüpunkt zum letzten zurückspringen
        public void SelectPrev()
        {
            if (curMenuItem > 0)
            {
                curMenuItem--;
            }
            else
            {
                curMenuItem = menuItemCount - 1;
            }
        }

        //Gibt das Indize des aktuellen Menüpunktes zurück
        public int GetSelectedNum()
        {
            return curMenuItem;
        }

        //Gibt den aktuellen Namen des aktuellen Menüpunktes zurück
        public string GetSelectedName()
        {
            return menuItems[curMenuItem];
        }

        //Updatet das Menü
        public void Update(GameTime gameTime)
        {
            //Durchläuft alle Menüpunkte
            for (int i = 0; i < menuItemCount; i++)
            {
                //Lässt die Skalierung des aktuellen Punktes langsam größer werden
                if (i == curMenuItem)
                {
                    if (scale[i] < 1.5f)
                    {
                        scale[i] += 0.04 + 10.0f * gameTime.ElapsedGameTime.Seconds;
                    }
                }
                //Lässt die Skalierung aller anderen Menüpunkte langsam kleiner werden
                else if (scale[i] > 1.0f && i != curMenuItem)
                {
                    scale[i] -= 0.04 + 10.0f * gameTime.ElapsedGameTime.Seconds;
                }
            }
        }

        //Methode um das komplette Menü zu zeichnen
        //Übergibt die Spritebatch und einen Boolschen Wert der abgibt ob sich das Menü außerhalb von
        //spriteBatch.Begin()/.End() befindet
        public void Draw(SpriteBatch spriteBatch, bool standalone)
        {
            if (standalone)
                spriteBatch.Begin();

            //Durchläuft alle Menüpunkte und zeichnet den Menüpunkt mit den entsprechenden Eigenschaften
            for (int i = 0; i < menuItemCount; i++)
            {
                if (i == curMenuItem)
                {
                    Vector2 p = pos[i];                 //Weist die Position einer temporären Variable zu
                    p.X -= (float)(22 * scale[i] / 2);  //Verschiebt die Position der x-Achse um den Namen mittig anzuzeigen
                    p.Y -= (float)(22 * scale[i] / 2);  //Verschiebt die Position der y-Achse um den Namen mittig anzuzeigen
                    spriteBatch.DrawString(font, menuItems[i], p, selected, 0.0f, new Vector2(0, 0), (float)scale[i], SpriteEffects.None, 0);
                }
                else
                {
                    Vector2 p = pos[i];                 //Weist die Position einer temporären Variable zu
                    p.X -= (float)(22 * scale[i] / 2);  //Verschiebt die Position der x-Achse um den Namen mittig anzuzeigen
                    p.Y -= (float)(22 * scale[i] / 2);  //Verschiebt die Position der y-Achse um den Namen mittig anzuzeigen
                    spriteBatch.DrawString(font, menuItems[i], p, unselected, 0.0f, new Vector2(0, 0), (float)scale[i], SpriteEffects.None, 0);
                }
            }

            if (standalone)
                spriteBatch.End();
        }
    }
}
