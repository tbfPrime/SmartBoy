using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SmartBoy
{
    public class PlayerConnector : System.Windows.Forms.AxHost, IOleServiceProvider, IOleClientSite
    {


        /// <summary>
        /// Used to attach the appropriate interface to Windows Media Player.
        /// In here, we call SetClientSite on the WMP Control, passing it
        /// the dotNet container (this instance.)
        /// </summary>
        protected override void AttachInterfaces()
        {
            try
            {
                //Get the IOleObject for Windows Media Player.
                IOleObject oleObject = this.GetOcx() as IOleObject;

                //Set the Client Site for the WMP control.
                oleObject.SetClientSite(this as IOleClientSite);

                return;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }


        #region IOleServiceProvider Memebers - Working
        /// <summary>
        /// During SetClientSite, WMP calls this function to get the pointer to <see cref="RemoteHostInfo"/>.
        /// </summary>
        /// <param name="guidService">See MSDN for more information - we do not use this parameter.</param>
        /// <param name="riid">The Guid of the desired service to be returned.  For this application it will always match
        /// the Guid of <see cref="IWMPRemoteMediaServices"/>.</param>
        /// <returns></returns>
        IntPtr IOleServiceProvider.QueryService(ref Guid guidService, ref Guid riid)
        {
            //If we get to here, it means Media Player is requesting our IWMPRemoteMediaServices interface
            if (riid == new Guid("cbb92747-741f-44fe-ab5b-f1a48f3b2a59"))
            {
                IWMPRemoteMediaServices iwmp = new RemoteHostInfo();
                return Marshal.GetComInterfaceForObject(iwmp, typeof(IWMPRemoteMediaServices));
            }

            throw new System.Runtime.InteropServices.COMException("No Interface", (int)HResults.E_NOINTERFACE);
        }
        #endregion

        #region IOleClientSite Members
        /// <summary>
        /// Not in use.  See MSDN for details.
        /// </summary>
        /// <exception cref="System.Runtime.InteropServices.COMException">E_NOTIMPL</exception>
        void IOleClientSite.SaveObject()
        {
            //throw new System.Runtime.InteropServices.COMException("Not Implemented", (int) HResults.E_NOTIMPL);
        }

        /// <summary>
        /// Not in use.  See MSDN for details.
        /// </summary>
        /// <exception cref="System.Runtime.InteropServices.COMException"></exception>
        object IOleClientSite.GetMoniker(uint dwAssign, uint dwWhichMoniker)
        {
            throw new System.Runtime.InteropServices.COMException("Not Implemented", (int)HResults.E_NOTIMPL);
        }

        /// <summary>
        /// Not in use.  See MSDN for details.
        /// </summary>
        /// <exception cref="System.Runtime.InteropServices.COMException"></exception>
        object IOleClientSite.GetContainer()
        {
            throw new System.Runtime.InteropServices.COMException("Not Implemented", (int)HResults.E_NOTIMPL);
        }

        #endregion


        public PlayerConnector() : base("6bf52a52-394a-11d3-b153-00c04f79faa6") { }

    }
}
