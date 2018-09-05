using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;
/*Exemple en long

Time Frame
H1 et H4


Tendance
1 . Identifier la tendance principale afin d’avoir un biais
C ’est pas évident à programmer. On pourrait en faire fi pour les premiers tests

#Entrée

#Deux stochatiques
1 = K21
2 = 5K

#Lorsque les deux stochastiques sont en survente, on entre la première position

Stop Loss ( encore en réflexion)
ADR x 100% (plus simple pour toi je crois)
ou
Dernier support + 1x ATR (ce qui serait plus juste son moi, mais pas facilement automatisable)

Cible
0 ,5 R

Positionnement MLOE
On divise la distance du stop en 4
Si le prix va à l’encontre de la cible on ajoute des positions pour un maximum de 4

Ainsi , pour un SL de 40pips, on aura des positions à chaque 10 pips (voir calculateur Excel)

Gestion du risque
On multiplie par 1,5 le pourcentage du capital à risque à chacune des entrée et on ajuste les lots en conséquence. Risque total pour les 4 trades dans mon exemple 2,03%
*/
namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class MLOE : Robot
    {
        [Parameter(DefaultValue = 0.0)]
        public double Parameter { get; set; }

        protected override void OnStart()
        {
            // Put your initialization logic here
        }

        protected override void OnTick()
        {
            // Put your core logic here
        }

        protected override void OnBar()
        {

        }


        protected override void OnStop()
        {
            // Put your deinitialization logic here
        }

        private void ExecuteOrder(double quantity, TradeType tradeType)
        {

        }
    }
}
