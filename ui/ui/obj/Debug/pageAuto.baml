   M S B A M L   `   `   `  ����=  9ui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:uiui  5      A =libPLC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:libPLC;assembly=libPLClibPLC 5      F BCalcBinding, Version=2.5.2.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:CalcBinding;assembly=CalcBindingCalcBinding 5          ui.pageAuto   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      "localclr-namespace:ui     5      2p$clr-namespace:libPLC;assembly=libPLC     5      <c.clr-namespace:CalcBinding;assembly=CalcBinding     5        N� Title$
  Auto��5
       
  frameW#��[  6    
 frameH#��[ 64   $
��Left=�6V   $	��Top=�6q    N� 	KeepAlive .   defaultStylePage#��[ 5      .��5      �� .+�5   
     libPLC.plcIndicator  
   Img$ hataseis=�6A    �� Left$	 876��6P   
 �� Top$ 11��6b      Input 6   �� *plc[1].tags[.bEstop]+6b   �� 5&   
   $	��167��6�   $	 804��6�   $	 303��6�   $��28��6�   
���   ����	#FFFFF7F7$��20(�6�   ��6   �� *plc[1].tags[.sProgram].Val+ �� StringFormat$ 	{0:0.###}�� �� UpdateSourceTrigger$ PropertyChanged=�6�     libPLC.recipeControl  5(   
   -   5          Name$ recipeC��5(   B   	   DgvType$	 recipe=�61   $	��429��6�   $	��435��6�   

   Plc
 6   �� *plc[1]+   
DirControl 6S   ��  �� ElementName$
 dirC��   FilePath 6{   �� *SelectedFile+$
 dirC��6�     libPLC.dirControl  5)   
   -   5          Name$
 dirC��5)   5      IniName$
 auto��6   
   Dir$ recipes��6'   $	��179��6C   $	��416��6O   $	 452��6\   $ 10��6n     libPLC.previewControl  5*   
   $	��977��6v   $��46��6�   $	 434��6�      RecipeControl 6   �� *
recipeCtrl+   DataHasChanged 6@   �� *recipeCtrl.DataHasChanged+��5+      �� 5,       �� EndPoint$ 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5-      �  � Color$ 	#FFC3D7E2��6#    � Offset$ 0Q�65   � 5.      $ 1Q�6#   5/      51      �� 53   
   -   5         ��Name$ buttonSendData��53      $��Lataa ohjelma��6)   $	 647��6A   $	 346��6S   $	��330��6d   $��80��6p   6|   �� 5:   
   -   5         ��Name$ checkSettings��5:      $	 647��6�   $ 11��6�    d� 	IsChecked$
 TrueY�6�    J� RenderTransformOrigin$ 7.883,-0.691&�6�   $	��179��6�   ��6*   ��  �� Path$&  settingsDirCtrl.SelectedFileName�$ Use setup :  {0}��6�   �� 5;   
   -   5         ��Name$ label��5;      $��Ladattu ohjelma : ��6   $	 667��6<   $	 303��6N     5<   
      Text$ 	Valoverho��6   $	 875��6X   $	 126��6j    6*   �� *plc[1].tags[.bLightCurtain]+6j   �� 5=   
   $	��167��6�   $	 804��6�   $	 264��6�   $��28��6�   
���   ����	#FFFFF7F7$��20(�6�   ��6   �� **(plc[1].tags[.Axis.nctoplc.ErrorCode].Val+$ 	{0:0.###}��$ PropertyChanged=�6�   �� 5>   
   -   5        $ 
label_Copy��5>      $��Moottorin Tilakoodi : ��6$   $	 647��6E   $	 264��6W   �� 5?   
   -   5        $ label_Copy1��5?      $�� ��6%   $	 646��60   $	 231��6B   �� 5@   
   $	��324��6g   $	 647��6t   $	 226��6�   $��28��6�   
���   ����	#FFFFF7F7$��20(�6�   ��6   �� *plc[1].tags[.sStateMsg].Val+$ PropertyChanged=�6�     5A   
   $ Moottori��6   $	 874��6W   $	 158��6i    6)   �� *plc[1].tags[.bEnableStatus]+6i   5T      5U      