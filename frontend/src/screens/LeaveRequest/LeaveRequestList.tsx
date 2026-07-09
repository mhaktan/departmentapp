import React, { useState, useCallback } from 'react';
import { TkButton } from '@takeoff-ui/react';
import { useListQuery } from '../../shared/useListQuery';
import { useDeleteMutation } from '../../shared/useDeleteMutation';
import { DeleteConfirmDialog } from '../../shared/DeleteConfirmDialog';
import { ListPageLayout } from '../../shared/ListPageLayout';
import type { TableColumn } from '../../shared/ListPageLayout';
import { actionColumn } from '../../shared/ActionButtons';
import { LeaveRequestCreate } from './LeaveRequestCreate';
import { LeaveRequestEdit } from './LeaveRequestEdit';
import { useFlows } from '../../flows/FlowProvider';

// ---------------------------------------------------------------------------
// Types
// ---------------------------------------------------------------------------

type LeaveRequestRecord = {
  id: string | number;
  startDate: string;
  endDate: string;
  totalDays: number;
  reason?: string;
  status: string;
  revisionNote?: string;
  requiresHRApproval: boolean;
  managerApproverId?: number;
  hrApproverId?: number;
  balanceDeducted?: boolean;
  employeeId: string;
  leaveTypeId: string;
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
  { field: 'startDate', header: 'Start Date', sortable: true, filterType: 'datepicker', html: (row: Record<string, unknown>) => row.startDate ? new Date(String(row.startDate)).toLocaleDateString() : '—' },
  { field: 'endDate', header: 'End Date', sortable: true, filterType: 'datepicker', html: (row: Record<string, unknown>) => row.endDate ? new Date(String(row.endDate)).toLocaleDateString() : '—' },
  { field: 'totalDays', header: 'Total Days', sortable: true },
  { field: 'reason', header: 'Reason', sortable: true, searchable: true, filterType: 'text' },
  { field: 'status', header: 'Status', sortable: true, filterType: 'radio', filterOptions: [{ label: 'Draft', value: '0' }, { label: 'PendingManagerApproval', value: '1' }, { label: 'PendingHRApproval', value: '2' }, { label: 'Approved', value: '3' }, { label: 'Revision', value: '4' }, { label: 'Cancelled', value: '5' }, { label: 'Rejected', value: '6' }], html: (row: Record<string, unknown>) => { const m: Record<string, string> = {'0': 'Draft', '1': 'PendingManagerApproval', '2': 'PendingHRApproval', '3': 'Approved', '4': 'Revision', '5': 'Cancelled', '6': 'Rejected'}; return m[String(row.status ?? '')] ?? String(row.status ?? '\u2014'); } },
  { field: 'revisionNote', header: 'Revision Note', sortable: true, searchable: true, filterType: 'text' },
  { field: 'requiresHRApproval', header: 'Requires H R Approval', sortable: true, filterType: 'checkbox', filterOptions: [{ label: 'Yes', value: 'true' }, { label: 'No', value: 'false' }], html: (row: Record<string, unknown>) => row.requiresHRApproval ? '<span style="color:#2e7d32;font-weight:600">Yes</span>' : '<span style="color:#999">No</span>' },
  { field: 'managerApproverId', header: 'Manager Approver Id', sortable: true },
  { field: 'hrApproverId', header: 'Hr Approver Id', sortable: true },
  { field: 'balanceDeducted', header: 'Balance Deducted', sortable: true, filterType: 'checkbox', filterOptions: [{ label: 'Yes', value: 'true' }, { label: 'No', value: 'false' }], html: (row: Record<string, unknown>) => row.balanceDeducted ? '<span style="color:#2e7d32;font-weight:600">Yes</span>' : '<span style="color:#999">No</span>' },
  { field: 'employeeId', header: 'Personel', sortable: true },
  { field: 'leaveTypeId', header: 'İzin Türü', sortable: true },
];

// ---------------------------------------------------------------------------
// LeaveRequestList
// ---------------------------------------------------------------------------

export const LeaveRequestList: React.FC = () => {
  const list = useListQuery<LeaveRequestRecord>({ resource: 'LeaveRequest' });
  const [showCreate, setShowCreate] = useState(false);
  const [editRecord, setEditRecord] = useState<LeaveRequestRecord | null>(null);
  const [selectedRows, setSelectedRows] = useState<LeaveRequestRecord[]>([]);
  const { triggerFlows } = useFlows();
  const del = useDeleteMutation('LeaveRequest', (ids) => { ids.forEach(id => triggerFlows('delete', 'LeaveRequest', { id })); });


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
        title="LeaveRequest"
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
        onSelectionChange={(rows) => setSelectedRows(rows as LeaveRequestRecord[])}
        onCrudAction={handleCrudAction}
        headerActions={<>
          {selectedRows.length > 0 && (
            <TkButton label={`Delete (${selectedRows.length})`} variant="danger" onTkClick={() => del.requestDelete(selectedRows.map(r => r.id), `${selectedRows.length} record(s)`)} />
          )}

          <TkButton label="+ Create LeaveRequest" variant="primary" onTkClick={() => setShowCreate(true)} />
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
      <LeaveRequestCreate open={showCreate} onClose={() => setShowCreate(false)} onSuccess={list.invalidate} />
      <LeaveRequestEdit record={editRecord} onClose={() => setEditRecord(null)} onSuccess={list.invalidate} />

    </>
  );
};

