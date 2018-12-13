<%@ Register Assembly="ASPNetSpell" Namespace="ASPNetSpell" TagPrefix="ASPNetSpell" %> 
 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml"> 
<head runat="server"> 
    <title></title> 
</head> 
<body> 
    <form id="form1" runat="server"> 
    <div> 
        <ASPNetSpell:SpellTextBox ID="SpellTextBox1" runat="server" >Hello Worldd. 
            </ASPNetSpell:SpellTextBox> 
        <ASPNetSpell:SpellButton ID="SpellButton1" runat="server" /> 
    </div> 
    </form> 
</body> 
</html>