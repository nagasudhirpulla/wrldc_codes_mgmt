using Ardalis.SmartEnum;

namespace Core.Enums;

public sealed class ElementType : SmartEnum<ElementType, string>
{
    public static readonly ElementType Bay = new(nameof(Bay), "Bay");
    public static readonly ElementType Bus = new(nameof(Bus), "BUS");
    public static readonly ElementType BusReactor = new(nameof(BusReactor), "BUS_REACTOR");
    public static readonly ElementType Compensator = new(nameof(Compensator), "COMPENSATOR");
    public static readonly ElementType Fsc = new(nameof(Fsc), "FSC");
    public static readonly ElementType GeneratingUnit = new(nameof(GeneratingUnit), "GENERATING_UNIT");
    public static readonly ElementType HvdcLineCkt = new(nameof(HvdcLineCkt), "HVDC_LINE_CIRCUIT");
    public static readonly ElementType HvdcPole = new(nameof(HvdcPole), "HVDC_POLE");
    public static readonly ElementType LineReactor = new(nameof(LineReactor), "LINE_REACTOR");
    public static readonly ElementType Transformer = new(nameof(Transformer), "TRANSFORMER");
    public static readonly ElementType TransmissionLineCkt = new(nameof(TransmissionLineCkt), "AC_TRANSMISSION_LINE_CIRCUIT");
    
    private ElementType(string name, string value) : base(name, value)
    {
    }
}
