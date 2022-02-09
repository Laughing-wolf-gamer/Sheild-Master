using System;
public interface IronSourceImpressionDataListener
{
    event Action<IronSourceImpressionData> OnImpressionDataReady;
}
