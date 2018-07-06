#ifndef M2_EDD_HEADER
#define M2_EDD_HEADER
#include <iparamb2.h>
#include <vector>

class EDDEntry {
private:
	int lodCount;
	std::vector<std::wstring> lodNames;
	Matrix3 matrix;
public:
	EDDEntry();
	~EDDEntry();
	void SetLodCount(int count);
	void SetMatrix(Matrix3 matrix);
	void SetLodNames(std::vector<std::wstring> lodNames);
	int GetLodCount();
	Matrix3 GetMatrix();
	std::vector<std::wstring> GetLodNames();

	void ReadFromStream(FILE* stream);
};
#endif