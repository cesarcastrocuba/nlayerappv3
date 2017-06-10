namespace NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg
{
    using NLayerApp.Domain.Seedwork;

    /// <summary>
    /// Picture of customer
    /// </summary>
    public class Picture
        :EntityGuid
    {
        #region Properties

        /// <summary>
        /// Get the raw of photo
        /// </summary>
        public byte[] RawPhoto{ get; set; }

        #endregion
    }
}
