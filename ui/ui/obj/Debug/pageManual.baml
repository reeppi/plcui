   M S B A M L   `   `   `  ����=  9ui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:uiui  5      A =libPLC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/$clr-namespace:libPLC;assembly=libPLClibPLC 5      F BCalcBinding, Version=2.5.2.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:CalcBinding;assembly=CalcBindingCalcBinding 5          ui.pageManual   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      "localclr-namespace:ui     5      2p$clr-namespace:libPLC;assembly=libPLC     5      <c.clr-namespace:CalcBinding;assembly=CalcBinding     5        N� Title$
  Auto��5
       
  frameW#��[  6    
 frameH#��[ 64   $
��Left=�6V   $	��Top=�6q    N� 	KeepAlive .   defaultStylePage#��[ 5      .��5      ��  � DataContext 6   �� *plc[1]+.+�5   
     libPLC.plcButton     Text$ Rulla 1��6   
   Img$ cylinder=�6G      CType$ toggle=�6W    �� Left$ 43��6g   
 �� Top$ 43��6x   $	��155��6�   $	��128��6�      Input 6%   �� *tags[.oRoller1]+6�     5   
   $ Rulla 2��6   $ cylinder=�6G   $ toggle=�6W   $	 198��6g   $ 43��6y   $	��155��6�   $	��128��6�    6%   �� *tags[.oRoller2]+6�     5   
   $ Rulla 3��6   $ cylinder=�6G   $ toggle=�6W   $	 354��6g   $ 43��6y   $	��155��6�   $	��128��6�    6%   �� *tags[.oRoller3]+6�     5   
   $ Rulla 4��6   $ cylinder=�6G   $ toggle=�6W   $	 509��6g   $ 43��6y   $	��155��6�   $	��128��6�    6%   �� *tags[.oRoller4]+6�     5   
   $ Rulla 5��6   $ cylinder=�6G   $ toggle=�6W   $	 665��6g   $ 43��6y   $	��155��6�   $	��128��6�    6%   �� *tags[.oRoller5]+6�     5   
   $ Rulla 6��6   $ cylinder=�6G   $ toggle=�6W   $	 820��6g   $ 43��6y   $	��155��6�   $	��128��6�    6%   �� *tags[.oRoller6]+6�     libPLC.plcIndicator  5   
   	   Text$	  -B1 Rulla1��6   $ 48��6N   $	 171��6_   
   Input
 6,   �� *tags[.iRoller1]+6_     5   
   $	  -B2 Rulla2��6   $	 203��6N   $	 171��6`   
 6,   �� *tags[.iRoller2]+6`     5   
   $	  -B3 Rulla3��6   $	 354��6N   $	 171��6`   
 6,   �� *tags[.iRoller3]+6`     5   
   $	  -B4 Rulla4��6   $	 514��6N   $	 171��6`   
 6,   �� *tags[.iRoller4]+6`     5   
   $	  -B5 Rulla5��6   $	 665��6N   $	 171��6`   
 6,   �� *tags[.iRoller5]+6`     5   
   $	  -B6 Rulla6��6   $	 820��6N   $	 171��6`   
 6,   �� *tags[.iRoller6]+6`     5   
   $ Axis FW��6   $ fw=�6G   $ press=�6Q   $ 48��6`   $	 218��6q   $	��155��6�   $	��128��6�    6%   �� *tags[.bDriveFW]+6�     5   
   $ Axis RW��6   $ rw=�6G   $ press=�6Q   $	 208��6`   $	 218��6r   $	��155��6�   $	��128��6�    6%   �� *tags[.bDriveRW]+6�   �� 5!   
   $	��149��6a   $ 54��6n   $	 351��6   $��28��6�   
���   ����	#FFFFF7F7$��20(�6�   ��6     CalcBinding.Binding  * tags[.Axis.nctoplc.actpos].Val+   StringFormat$ 	{0:0.###}��6�     5"   
   $ Pukkari��6   $ cylinder=�6F   $ toggle=�6V   $	 820��6f   $	 218��6x   $	��155��6�   $	��128��6�    6%   �� *tags[.oPusher]+6�     5#   
   $
	  +B7��6   $	 743��6J   $	 218��6\   
 6%   �� *tags[.iPusherAOut]+6\     5$   
   $
	  +B9��6   $	 743��6J   $	 250��6\   
 6%   �� *tags[.iPusherBOut]+6\     5%   
   $
	  -B8��6   $	 743��6I   $	 279��6[   
 6%   �� *tags[.iPusherAIn]+6[     5&   
   $	  -B10��6   $	 743��6J   $	 311��6\   
 6&   �� *tags[.iPusherBIn]+6\     5'   
   $ Kelkka Pukkari��6   $ cylinder=�6Q   $ toggle=�6a   $	 529��6q   $	 389��6�   $	��177��6�   $��49��6�   $y�Hidden=�6�    6,   �� *tags[.oGripPusher]+6�     5(   
   $ Tunnistin Sylinteri��6   $ cylinder=�6R   $ toggle=�6b   $	 529��6r   $	 333��6�   $	��177��6�   $��49��6�    61   �� *tags[.oSlider]+6�     5)   
   $ Pihti 1 etu��6   $ cylinder=�6L   $ toggle=�6\   $	 529��6l   $	 225��6~   $	��177��6�   $��49��6�    6)   �� *tags[.oGripper1]+6�     5*   
   $ Pihti 2 taka��6   $ cylinder=�6M   $ toggle=�6]   $	 529��6m   $	 279��6   $	��177��6�   $��49��6�    6*   �� *tags[.oGripper2]+6�     5+   
   $	 B12 Tunnistin 2��6   $	 389��6V   $	 289��6h   $	��134��6y   
 60   �� *tags[.iSliderGrip2]+6y     5,   
   $	 B11 Tunnistin 1��6   $	 389��6V   $	 234��6h   $	��134��6y   
 60   �� *tags[.iSliderGrip1]+6y   54      59      