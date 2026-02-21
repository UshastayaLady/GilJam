// Auto-generated EntryPoint for scene
// SceneGUID: 198cd3e7b0525334c912d5f816a6a235
using UnityEngine;
using System.Collections.Generic;
using WebUtility;

public class PigEntryPoint : AbstractEntryPoint
{
   protected override List<IDIRouter> Routers => new List<IDIRouter>()
   {
      new SDKAdapterRouter(),
      new PaymentRouter(),
      new WallRouter(),
       new PigRouter(),
       new InventoryRouter()
   };
}
