   M S B A M L   `   `   `  ����=  9ui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:uiui  5      A =libPLC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:libPLC;assembly=libPLClibPLC 5          ui.Page3   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      "localclr-namespace:ui     5      2p$clr-namespace:libPLC;assembly=libPLC     5      -   5          N�Name$
  page��5       
  frameW#��[  5	       
 frameH#��[ 6&    N� Title$ Page1��5
      $
��Left=�5      $	��Top=�5       N� 	KeepAlive .   defaultStylePage#��[ 5      .��5      �� .+�5   
     libPLC.recipeControl  -   5          Name$ 
variablesC��5         DgvType$ 	variables=�60   $	��480��6�   $	��706��6�      
DirControl 6E   ��  �� ElementName$
 dirC��   FilePath 6m   �� *SelectedFile+$
 dirC��6�     libPLC.dirControl  5   
   -   5          Name$
 dirC��5      	   IniName$		 PLC��6&   

   Dir$	
 plc��65    �� Left$	 807��6?   
 �� Top$ 10��6Q   $	��103��6a   $	��119��6n   �� 5   
   -   5         ��Name$ buttonReloadConnection��5      $��Reload��6V   $	 807��6g   $	 405��6y   $	��119��6�   $��42��6�   �� 5   
   -   5        $ buttonRefresh��5      $��Refresh��6(   $	 807��6:   $	 361��6L   $	��119��6]   $��37��6i   6u   �� 5   
   -   5        $ buttonAddWatch��5      $��AddWatch��6)   $��40��6<   $	 807��6H   $	 273��6Z   $	��119��6k   6w   �� 5   
   -   5        $ buttonAddChart��5      $��AddScope��6)   $��40��6<   $	 807��6H   $	 316��6Z   $	��119��6k   6w     libPLC.plcButton  5   
      Text$  Save 
persistent 
PLC-data��6   
   Img$ 	indicator=�6h      CType$ press=�6y   $	 807��6�   $	 152��6�   $	��119��6�   $��82��6�      Input 6B   �� *plc[1].tags[.bSave]+6�   5      5      