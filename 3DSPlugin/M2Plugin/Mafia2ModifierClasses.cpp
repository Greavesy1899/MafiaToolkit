#include "Mafia2ModifierClasses.h"

#define PBLOCK_REF	0

class M2ModifierClassDesc : public ClassDesc2
{
public:
	virtual int IsPublic() { return TRUE; }
	virtual void* Create(BOOL /*loading = FALSE*/) { return new M2Modifier(); }
	virtual const TCHAR *	ClassName() { return GetString(IDS_M2_MODIFIER); }
	virtual SClass_ID SuperClassID() { return OSM_CLASS_ID; }
	virtual Class_ID ClassID() { return M2_MODIFIER_CLASS_ID; }
	virtual const TCHAR* Category() { return GetString(IDS_CATEGORY); }

	virtual const TCHAR* InternalName() { return _T("M2 Modifier"); }	// returns fixed parsable name (scripter-visible name)
	virtual HINSTANCE HInstance() { return hInstance; }					// returns owning module handle
};

ClassDesc2* GetM2ModifierClassDesc() {
	static M2ModifierClassDesc M2ModifierDesc;
	return &M2ModifierDesc;
}

enum { maxproject6_params };
enum {
	pb_spin,
};

class BendDlgProc : public ParamMap2UserDlgProc {
public:
	INT_PTR DlgProc(TimeValue t, IParamMap2* map, HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);
	void DeleteThis() {}
};

static BendDlgProc theBendProc;


INT_PTR BendDlgProc::DlgProc(TimeValue t, IParamMap2 *map, HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam) {
	return FALSE;
}

static ParamBlockDesc2 maxproject6_param_blk(maxproject6_params, _T("Model Parameters"), 0, GetM2ModifierClassDesc(), P_AUTO_CONSTRUCT + P_AUTO_UI, PBLOCK_REF,
	//rollout
	IDD_PANEL, IDS_PARAMS, 0, 0, NULL,
	// params
	pb_spin, _T("spin"), TYPE_FLOAT, P_ANIMATABLE, IDS_SPIN,
	p_default, 0.1f,
	p_range, 0.0f, 1000.0f,
	p_ui, TYPE_SPINNER, EDITTYPE_FLOAT, IDC_EDIT, IDC_SPIN, 0.01f,
	p_end,
	p_end
);

M2Modifier::M2Modifier() : pblock(nullptr) {
	GetM2ModifierClassDesc()->MakeAutoParamBlocks(this);
}
M2Modifier::~M2Modifier() {
}
void M2Modifier::SetReference(int i, RefTargetHandle rtarg) {
	if (i == PBLOCK_REF)
	{
		pblock = (IParamBlock2*)rtarg;
	}
}

RefTargetHandle M2Modifier::GetReference(int i) {
	if (i == PBLOCK_REF)
	{
		return pblock;
	}
	return nullptr;
}
void M2Modifier::ModifyObject(TimeValue /*t*/, ModContext& /*mc*/, ObjectState* /*os*/, INode* /*node*/) {
#pragma message(TODO("Add the code for actually modifying the object"))
}
void M2Modifier::NotifyInputChanged(const Interval &/*changeInt*/, PartID /*partID*/, RefMessage /*message*/, ModContext* /*mc*/) {
}
void M2Modifier::NotifyPreCollapse(INode* /*node*/, IDerivedObject* /*derObj*/, int /*index*/) {
#pragma message(TODO("Perform any Pre Stack Collapse methods here"))
}
void M2Modifier::NotifyPostCollapse(INode* /*node*/, Object* /*obj*/, IDerivedObject* /*derObj*/, int /*index*/) {
#pragma message(TODO("Perform any Post Stack collapse methods here."))
}
Interval M2Modifier::LocalValidity(TimeValue /*t*/) {
	// if being edited, return NEVER forces a cache to be built 
	// after previous modifier.
	if (TestAFlag(A_MOD_BEING_EDITED))
		return NEVER;
#pragma message(TODO("Return the validity interval of the modifier"))
	return NEVER;
}
void M2Modifier::BeginEditParams(IObjParam* ip, ULONG flags, Animatable* prev) {
	TimeValue t = ip->GetTime();
	NotifyDependents(Interval(t, t), PART_ALL, REFMSG_BEGIN_EDIT);
	NotifyDependents(Interval(t, t), PART_ALL, REFMSG_MOD_DISPLAY_ON);
	SetAFlag(A_MOD_BEING_EDITED);
	GetM2ModifierClassDesc()->BeginEditParams(ip, this, flags, prev);
}

void M2Modifier::EndEditParams(IObjParam *ip, ULONG flags, Animatable *next) {
	GetM2ModifierClassDesc()->EndEditParams(ip, this, flags, next);

	TimeValue t = ip->GetTime();
	ClearAFlag(A_MOD_BEING_EDITED);
	NotifyDependents(Interval(t, t), PART_ALL, REFMSG_END_EDIT);
	NotifyDependents(Interval(t, t), PART_ALL, REFMSG_MOD_DISPLAY_OFF);
}
BOOL M2Modifier::HasUVW() {
#pragma message(TODO("Return whether the object has UVW coordinates or not"))
	return TRUE;
}
RefResult M2Modifier::NotifyRefChanged(
	const Interval& /*changeInt*/, RefTargetHandle hTarget,
	PartID& /*partID*/, RefMessage message, BOOL /*propagate*/) {
#pragma message(TODO("Add code to handle the various reference changed messages"))
	switch (message)
	{
	case REFMSG_TARGET_DELETED:
	{
		if (hTarget == pblock)
		{
			pblock = nullptr;
		}
	}
	break;
	}
	return REF_SUCCEED;
}
void M2Modifier::SetGenUVW(BOOL sw) {
	if (sw == HasUVW())
		return;
#pragma message(TODO("Set the plugin internal value to sw"))
}
IOResult M2Modifier::Load(ILoad* /*iload*/) {
#pragma message(TODO("Add code to allow plugin to load its data"))

	return IO_OK;
}
IOResult M2Modifier::Save(ISave* /*isave*/) {
#pragma message(TODO("Add code to allow plugin to save its data"))

	return IO_OK;
}
Interval M2Modifier::GetValidity(TimeValue /*t*/) {
	Interval valid = FOREVER;
#pragma message(TODO("Return the validity interval of the modifier"))
	return valid;
}
RefTargetHandle M2Modifier::Clone(RemapDir& remap) {
	M2Modifier* newmod = new M2Modifier();
#pragma message(TODO("Add the cloning code here"))
	newmod->ReplaceReference(PBLOCK_REF, remap.CloneRef(pblock));
	BaseClone(this, newmod, remap);
	return(newmod);
}