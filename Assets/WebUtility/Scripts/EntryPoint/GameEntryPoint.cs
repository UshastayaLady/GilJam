// Auto-generated EntryPoint for scene
// SceneGUID: 996bebcb32e58e64f9b9af0262438342
using UnityEngine;
using System.Collections.Generic;
using WebUtility;

public class GameEntryPoint : AbstractEntryPoint
{
   protected override List<IDIRouter> Routers => new List<IDIRouter>()
   {
      new SDKAdapterRouter(),
      new PaymentRouter(),
      new SeedbedRouter(),
      new WallRouter(),
      new PigRouter(),
      new InventoryRouter()
   };
}
