import React, { useState, useCallback } from 'react';
import { TkButton } from '@takeoff-ui/react';
import { useListQuery } from '../../shared/useListQuery';
import { useDeleteMutation } from '../../shared/useDeleteMutation';
import { DeleteConfirmDialog } from '../../shared/DeleteConfirmDialog';
import { ListPageLayout } from '../../shared/ListPageLayout';
import type { TableColumn } from '../../shared/ListPageLayout';
import { actionColumn } from '../../shared/ActionButtons';
import { EmployeeCreate } from './EmployeeCreate';
import { EmployeeEdit } from './EmployeeEdit';
import { useFlows } from '../../flows/FlowProvider';

// ---------------------------------------------------------------------------
// Types
// ---------------------------------------------------------------------------

type EmployeeRecord = {
  id: string | number;
  employeeNumber: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  birthDate?: string;
  gender?: string;
  nationalId?: string;
  address?: string;
  hireDate: string;
  terminationDate?: string;
  jobTitle?: string;
  employmentType: string;
  status: string;
  emergencyContactName?: string;
  emergencyContactPhone?: string;
  emergencyContactRelation?: string;
  bankAccountNumber?: string;
  bankName?: string;
  taxNumber?: string;
  socialSecurityNumber?: string;
  annualLeaveBalance?: number;
  notes?: string;
  departmentId: string;
  branchId: string;
  employeeId: string;
  onboardingId: string;
  [key: string]: unknown;
};

// ---------------------------------------------------------------------------
// Column definition — edit this array to add/remove/reorder columns
// ---------------------------------------------------------------------------
//
// Override examples:
//   • Hide a column:     remove its entry from COLUMNS
//   • Add a custom col:  { field: 'fullName', header: 'Full Name', html: (row) => `${row.firstName} ${row.lastName}` }
//   • Enable filtering:  add searchable: true  or  filterType: 'text' | 'checkbox' | 'radio' | 'datepicker'
//   • Custom cell render: html: (row) => `<span style="color:green">${row.status}</span>`
//
// Shared components (src/shared/) can be edited to change behavior globally:
//   • ListPageLayout  — table wrapper, pagination, header layout
//   • ActionButtons   — edit/delete button styles, labels, and behavior
//   • useListQuery    — data fetching, sorting, filtering logic
//   • useDeleteMutation / DeleteConfirmDialog — delete flow
//
// Action buttons override: edit src/shared/ActionButtons.ts DEFAULT_CONFIG
// or pass custom config: actionColumn('id', { hasEdit: true, config: { edit: { label: 'View', style: '...' } } })
//

const COLUMNS: TableColumn[] = [
  { field: 'id', header: 'ID', sortable: true },
  { field: 'employeeNumber', header: 'Employee Number', sortable: true, searchable: true, filterType: 'text' },
  { field: 'firstName', header: 'First Name', sortable: true, searchable: true, filterType: 'text' },
  { field: 'lastName', header: 'Last Name', sortable: true, searchable: true, filterType: 'text' },
  { field: 'email', header: 'Email', sortable: true, searchable: true, filterType: 'text' },
  { field: 'phone', header: 'Phone', sortable: true, searchable: true, filterType: 'text' },
  { field: 'birthDate', header: 'Birth Date', sortable: true, filterType: 'datepicker', html: (row: Record<string, unknown>) => row.birthDate ? new Date(String(row.birthDate)).toLocaleDateString() : '—' },
  { field: 'gender', header: 'Gender', sortable: true, filterType: 'radio', filterOptions: [{ label: 'Male', value: '0' }, { label: 'Female', value: '1' }, { label: 'Other', value: '2' }], html: (row: Record<string, unknown>) => { const m: Record<string, string> = {'0': 'Male', '1': 'Female', '2': 'Other'}; return m[String(row.gender ?? '')] ?? String(row.gender ?? '\u2014'); } },
  { field: 'nationalId', header: 'National Id', sortable: true, searchable: true, filterType: 'text' },
  { field: 'address', header: 'Address', sortable: true, searchable: true, filterType: 'text' },
  { field: 'hireDate', header: 'Hire Date', sortable: true, filterType: 'datepicker', html: (row: Record<string, unknown>) => row.hireDate ? new Date(String(row.hireDate)).toLocaleDateString() : '—' },
  { field: 'terminationDate', header: 'Termination Date', sortable: true, filterType: 'datepicker', html: (row: Record<string, unknown>) => row.terminationDate ? new Date(String(row.terminationDate)).toLocaleDateString() : '—' },
  { field: 'jobTitle', header: 'Job Title', sortable: true, searchable: true, filterType: 'text' },
  { field: 'employmentType', header: 'Employment Type', sortable: true, filterType: 'radio', filterOptions: [{ label: 'FullTime', value: '0' }, { label: 'PartTime', value: '1' }, { label: 'Contract', value: '2' }, { label: 'Intern', value: '3' }], html: (row: Record<string, unknown>) => { const m: Record<string, string> = {'0': 'FullTime', '1': 'PartTime', '2': 'Contract', '3': 'Intern'}; return m[String(row.employmentType ?? '')] ?? String(row.employmentType ?? '\u2014'); } },
  { field: 'status', header: 'Status', sortable: true, filterType: 'radio', filterOptions: [{ label: 'Active', value: '0' }, { label: 'OnLeave', value: '1' }, { label: 'Terminated', value: '2' }, { label: 'Suspended', value: '3' }], html: (row: Record<string, unknown>) => { const m: Record<string, string> = {'0': 'Active', '1': 'OnLeave', '2': 'Terminated', '3': 'Suspended'}; return m[String(row.status ?? '')] ?? String(row.status ?? '\u2014'); } },
  { field: 'emergencyContactName', header: 'Emergency Contact Name', sortable: true, searchable: true, filterType: 'text' },
  { field: 'emergencyContactPhone', header: 'Emergency Contact Phone', sortable: true, searchable: true, filterType: 'text' },
  { field: 'emergencyContactRelation', header: 'Emergency Contact Relation', sortable: true, searchable: true, filterType: 'text' },
  { field: 'bankAccountNumber', header: 'Bank Account Number', sortable: true, searchable: true, filterType: 'text' },
  { field: 'bankName', header: 'Bank Name', sortable: true, searchable: true, filterType: 'text' },
  { field: 'taxNumber', header: 'Tax Number', sortable: true, searchable: true, filterType: 'text' },
  { field: 'socialSecurityNumber', header: 'Social Security Number', sortable: true, searchable: true, filterType: 'text' },
  { field: 'annualLeaveBalance', header: 'Annual Leave Balance', sortable: true },
  { field: 'notes', header: 'Notes', sortable: true, searchable: true, filterType: 'text' },
  { field: 'departmentId', header: 'Departman', sortable: true },
  { field: 'branchId', header: 'Şube', sortable: true },
  { field: 'employeeId', header: 'Personel', sortable: true },
  { field: 'onboardingId', header: 'İşe Alım / Oryantasyon', sortable: true },
];

// ---------------------------------------------------------------------------
// EmployeeList
// ---------------------------------------------------------------------------

export const EmployeeList: React.FC = () => {
  const list = useListQuery<EmployeeRecord>({ resource: 'Employee' });
  const [showCreate, setShowCreate] = useState(false);
  const [editRecord, setEditRecord] = useState<EmployeeRecord | null>(null);
  const [selectedRows, setSelectedRows] = useState<EmployeeRecord[]>([]);
  const { triggerFlows } = useFlows();
  const del = useDeleteMutation('Employee', (ids) => { ids.forEach(id => triggerFlows('delete', 'Employee', { id })); });


  const columns = [...COLUMNS, ...actionColumn('id', { hasEdit: true, hasDelete: true })];

  const handleCrudAction = useCallback((action: string, id: string) => {
    const row = list.records.find((r) => String(r.id) === id);
    if (!row) return;
    if (action === 'edit') setEditRecord(row);
    if (action === 'delete') del.requestSingleDelete(row.id);
  }, [list.records]);

  return (
    <>
      <ListPageLayout
        title="Employee"
        subtitle={Object.keys(list.displayParams).length > 0 ? (
          <div style={{ fontSize: 13, color: '#666', marginTop: 4 }}>
            {Object.entries(list.displayParams).map(([k, v]) => (
              <span key={k} style={{ marginRight: 12 }}>{k}: <strong>{v}</strong></span>
            ))}
          </div>
        ) : undefined}
        records={list.records}
        columns={columns}
        dataKey="id"
        total={list.total}
        loading={list.isLoading}
        page={list.page}
        perPage={list.perPage}
        onPageChange={list.setPage}
        onPerPageChange={list.setPerPage}
        onTableRequest={list.handleTableRequest}
        selectionMode="checkbox"
        selectedRows={selectedRows}
        onSelectionChange={(rows) => setSelectedRows(rows as EmployeeRecord[])}
        onCrudAction={handleCrudAction}
        headerActions={<>
          {selectedRows.length > 0 && (
            <TkButton label={`Delete (${selectedRows.length})`} variant="danger" onTkClick={() => del.requestDelete(selectedRows.map(r => r.id), `${selectedRows.length} record(s)`)} />
          )}

          <TkButton label="+ Create Employee" variant="primary" onTkClick={() => setShowCreate(true)} />
        </>}
      />

      {list.isError && (
        <div style={{ padding: '10px 14px', background: '#fff3f3', border: '1px solid #f5c6c6', borderRadius: 6, color: '#c62828', fontSize: 13, marginBottom: 12 }}>
          Failed to load data: {(list.error as Error).message}
        </div>
      )}

      <DeleteConfirmDialog
        visible={!!del.deleteTarget}
        label={del.deleteTarget?.label ?? ''}
        isPending={del.isPending}
        onConfirm={del.confirmDelete}
        onCancel={() => del.setDeleteTarget(null)}
      />
      <EmployeeCreate open={showCreate} onClose={() => setShowCreate(false)} onSuccess={list.invalidate} />
      <EmployeeEdit record={editRecord} onClose={() => setEditRecord(null)} onSuccess={list.invalidate} />

    </>
  );
};

