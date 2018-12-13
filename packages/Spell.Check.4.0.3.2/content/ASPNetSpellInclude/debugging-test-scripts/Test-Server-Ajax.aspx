<%@ Register Assembly="ASPNetSpell" Namespace="ASPNetSpell" TagPrefix="ASPNetSpell" %> 
 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml"> 
<head runat="server"> 
    <title>ASPNetSpell - Server Debug Utility</title> 
</head> 
<body> 
    <form id="form1" runat="server"> 
    <div> 
    <p>This page allows you to debug server tranactions with ASPNetSpell</p>
    <script type="text/javascript">
        var LIVESPELL_DEBUG_MODE = true;
    </script>
    
        <ASPNetSpell:SpellTextBox ID="SpellTextBox1" runat="server" >Hello Worldd. 
            </ASPNetSpell:SpellTextBox> 
        <ASPNetSpell:SpellButton ID="SpellButton1" runat="server" /> 
    </div> 
    </form> 
</body> 
</html>