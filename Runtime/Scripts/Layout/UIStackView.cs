using System.Collections.Generic;
using UnityEngine;

namespace UIKit
{
	/// <summary>
	/// 여러 뷰를 수평/수직 정렬 배치하는 컨테이너 뷰.
	/// </summary>
    public class UIStackView : UIView
    {
	    public enum Axis
	    {
		    Horizontal,
		    Vertical,
	    }

	    public enum Alignment
	    {
		    Fill,
		    Leading,
		    Trailing,
		    Center,
		    Top,
		    Bottom,
		    FirstBaseline,
		    LastBaseline,
	    }

	    public enum Distribution
	    {
		    Fill,
		    FillEqually,
		    FillProportionally,
		    EqaulSpacing,
		    EqaulCentering,
	    }
	    
	    #region INSPECTOR
	    [SerializeField] private Axis m_Axis;
	    [SerializeField] private Alignment m_Alignment;
	    [SerializeField] private Distribution m_Distribution;
	    [SerializeField] private List<UIView> m_ArrangedChildren;
	    [SerializeField] private float m_Spacing;
	    [SerializeField] private bool m_IsBaselineRelativeArrangement;
	    [SerializeField] private bool m_IsLayoutMarginsRelativeArrangement;
	    #endregion
	    
	    public void AddC()
	    {
		    
	    }
    }
}
