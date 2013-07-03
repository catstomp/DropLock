using System;
using Terraria;
using Hooks;
using System.IO;
using TShockAPI;

namespace DropLock
{
    [APIVersion(1, 13)]
    public class DropLock : TerrariaPlugin
    {
        public override Version Version
        {
            get { return new Version(1,0,0); }
        }

        public override string Name
        {
            get { return "DropLock"; }
        }

        public override string Author
        {
            get { return "Antagonist and Ijwu"; }
        }

        public override string Description
        {
            get { return "An anti hack/cheat plugin"; }
        }

        public DropLock(Main game)
            : base(game)
        {
        }

        public override void Initialize()
        {
        	GameHooks.Initialize += OnInitialize;
        	NetHooks.GetData += OnGetData;
        }

        public void OnInitialize()
        {

        }
        
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				GameHooks.Initialize -= OnInitialize;
			}
			base.Dispose(disposing);
		}
		
		public void OnGetData(GetDataEventArgs e)
		{
            if (e.Handled)
                return;
			
			if (e.MsgID == PacketTypes.ItemDrop)
			{
                using (var data = new MemoryStream(e.Msg.readBuffer, e.Index, e.Length))
                {
                    var reader = new BinaryReader(data);
                    var id = reader.ReadInt16();
                    // Don't need the rest of this, but I'm leaving it here for future expansion!
                    //var posx = reader.ReadSingle();
                    //var posy = reader.ReadSingle();
                    //var velx = reader.ReadSingle();
                    //var vely = reader.ReadSingle();
                    //var stack = reader.ReadByte();
                    //var prefix = reader.ReadByte();
                    //var type = reader.ReadInt16();

                    if (id == 0)
                        return;

                    TSPlayer player = TShock.Players[e.Msg.whoAmI];
                    if (!player.Group.HasPermission("dropitem") && id == 0xC8)
                    {
                        e.Handled = true;
                        player.SendErrorMessage("You are not trusted enough to drop items.");
                    }
                }
			}
		}
    }
}