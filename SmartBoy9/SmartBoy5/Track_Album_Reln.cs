//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartBoy
{
    using System;
    using System.Collections.Generic;
    
    public partial class Track_Album_Reln
    {
        public string id { get; set; }
        public string MB_Track_ID { get; set; }
        public string MB_AlbumID { get; set; }
    
        public virtual Album_SB Album_SB { get; set; }
        public virtual Track_SB Track_SB { get; set; }
    }
}