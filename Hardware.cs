using System;

public class Hardware
{
    public string Section;
    public string Type;
    public string Firm;
    public string Model;
    public int Price;
    public string Comment;

    public Hardware(string newSection = "Не задано", string newType = "Не задано", string newFirm = "Не задано", string newModel = "Не задано", int newPrice = 0, string newComment = "Не задано")
    {
        Section = newSection;
        Type = newType;
        Firm = newFirm;
        Model = newModel;
        Price = newPrice;
        Comment = newComment;
    }

    public string Info()
    {
        string output;
        output = Type + "\r\nФирма: " + Firm + "\r\nМодель: " + Model + "\r\nЦена: " + Price.ToString() + "\r\nКомментарий:\r\n\"" + Comment + "\"\r\n";
        return output;
    }
}
