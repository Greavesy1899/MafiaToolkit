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

enum {
	m2_pblock 
};

enum {
	pb_spin, m2_hasnormals, m2_hastangents, m2_hasuvs
};

static M2ModifierMainDlgProc theM2ModifierMainDlgProc;

static ParamBlockDesc2 M2ModifierRollout(m2_pblock, _T("Model Parameters"), 0, GetM2ModifierClassDesc(), P_AUTO_CONSTRUCT + P_AUTO_UI, PBLOCK_REF,
	//rollout
	IDD_M2MODIFIERPANEL, IDS_PARAMS, 0, 0, NULL,

	m2_hasnormals, _T("hasNormals"), TYPE_BOOL, P_RESET_DEFAULT, IDS_ARA_S_DESC,
	p_default, false,
	p_ui, TYPE_SINGLECHECKBOX, IDC_HASNORMALS,
	p_end,

	m2_hastangents, _T("hasTangents"), TYPE_BOOL, P_RESET_DEFAULT, IDS_ARA_S_DESC,
	p_default, false,
	p_ui, TYPE_SINGLECHECKBOX, IDC_HASTANGENTS,
	p_end,

	m2_hasuvs, _T("hasUV"), TYPE_BOOL, P_RESET_DEFAULT, IDS_ARA_S_DESC,
	p_default, false,
	p_ui, TYPE_SINGLECHECKBOX, IDC_HASUVS,
	p_end,
	p_end
);

/*
	Create the modifier. This also sets the param block and the "HasNormals" and "HasUVs" to true.
*/
M2Modifier::M2Modifier() : pblock(nullptr) {
	GetM2ModifierClassDesc()->MakeAutoParamBlocks(this);
	pblock->SetValue(m2_hasnormals, 0, TRUE, 0);
	pblock->SetValue(m2_hasuvs, 0, TRUE, 0);
}

/*
	Modifier deconstructor. Calls nothing for now.
*/
M2Modifier::~M2Modifier() {
}

/*
	Set pblock as incoming reference.
*/
void M2Modifier::SetReference(int i, RefTargetHandle rtarg) {
	if (i == PBLOCK_REF)
	{
		pblock = (IParamBlock2*)rtarg;
	}
}

/*
	Retrieve the pblock by ID.
*/
RefTargetHandle M2Modifier::GetReference(int i) {
	if (i == PBLOCK_REF)
	{
		return pblock;
	}
	return nullptr;
}

/*
	TODO.
*/
void M2Modifier::ModifyObject(TimeValue /*t*/, ModContext& /*mc*/, ObjectState* /*os*/, INode* /*node*/) {
#pragma message(TODO("Add the code for actually modifying the object"))
}

/*
	TODO.
*/
void M2Modifier::NotifyInputChanged(const Interval &/*changeInt*/, PartID /*partID*/, RefMessage /*message*/, ModContext* /*mc*/) {
}

/*
	TODO.
*/
void M2Modifier::NotifyPreCollapse(INode* /*node*/, IDerivedObject* /*derObj*/, int /*index*/) {
#pragma message(TODO("Perform any Pre Stack Collapse methods here"))
}

/*
	TODO.
*/
void M2Modifier::NotifyPostCollapse(INode* /*node*/, Object* /*obj*/, IDerivedObject* /*derObj*/, int /*index*/) {
#pragma message(TODO("Perform any Post Stack collapse methods here."))
}

/*
	TODO.
*/
Interval M2Modifier::LocalValidity(TimeValue /*t*/) {
	// if being edited, return NEVER forces a cache to be built 
	// after previous modifier.
	if (TestAFlag(A_MOD_BEING_EDITED))
		return NEVER;
#pragma message(TODO("Return the validity interval of the modifier"))
	return NEVER;
}

/*
	TODO.
*/
void M2Modifier::BeginEditParams(IObjParam* ip, ULONG flags, Animatable* prev) {
	TimeValue t = ip->GetTime();
	NotifyDependents(Interval(t, t), PART_ALL, REFMSG_BEGIN_EDIT);
	NotifyDependents(Interval(t, t), PART_ALL, REFMSG_MOD_DISPLAY_ON);
	SetAFlag(A_MOD_BEING_EDITED);
	GetM2ModifierClassDesc()->BeginEditParams(ip, this, flags, prev);
}

/*
	TODO.
*/
void M2Modifier::EndEditParams(IObjParam *ip, ULONG flags, Animatable *next) {
	GetM2ModifierClassDesc()->EndEditParams(ip, this, flags, next);

	TimeValue t = ip->GetTime();
	ClearAFlag(A_MOD_BEING_EDITED);
	NotifyDependents(Interval(t, t), PART_ALL, REFMSG_END_EDIT);
	NotifyDependents(Interval(t, t), PART_ALL, REFMSG_MOD_DISPLAY_OFF);
}

/*
	TODO.
*/
BOOL M2Modifier::HasUVW() {
#pragma message(TODO("Return whether the object has UVW coordinates or not"))
	return TRUE;
}

/*
	TODO.
*/
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

/*
	TODO.
*/
void M2Modifier::SetGenUVW(BOOL sw) {
	if (sw == HasUVW())
		return;
#pragma message(TODO("Set the plugin internal value to sw"))
}

/*
	TODO.
*/
IOResult M2Modifier::Load(ILoad* /*iload*/) {
#pragma message(TODO("Add code to allow plugin to load its data"))

	return IO_OK;
}

/*
	TODO.
*/
IOResult M2Modifier::Save(ISave* /*isave*/) {
#pragma message(TODO("Add code to allow plugin to save its data"))

	return IO_OK;
}

/*
	TODO.
*/
Interval M2Modifier::GetValidity(TimeValue /*t*/) {
	Interval valid = FOREVER;
#pragma message(TODO("Return the validity interval of the modifier"))
	return valid;
}

/*
	TODO.
*/
RefTargetHandle M2Modifier::Clone(RemapDir& remap) {
	M2Modifier* newmod = new M2Modifier();
#pragma message(TODO("Add the cloning code here"))
	newmod->ReplaceReference(PBLOCK_REF, remap.CloneRef(pblock));
	BaseClone(this, newmod, remap);
	return(newmod);
}

/*
	TODO.
*/
void M2ModifierMainDlgProc::UpdateSelLevelDisplay(HWND hWnd) {
	if (!mod) return;
	UpdateWindow(hWnd);
}

/*
	TODO.
*/
void M2ModifierMainDlgProc::SetEnables(HWND hParams) {

}

/*
	TODO.
*/
INT_PTR M2ModifierMainDlgProc::DlgProc(TimeValue t, IParamMap2 *map, HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam) {
	return FALSE;
}
//WINDOW METHODS