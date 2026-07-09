import React, { useState, useCallback } from 'react';
import { TkButton } from '@takeoff-ui/react';
import { useListQuery } from '../../shared/useListQuery';
import { useDeleteMutation } from '../../shared/useDeleteMutation';
import { DeleteConfirmDialog } from '../../shared/DeleteConfirmDialog';
import { ListPageLayout } from '../../shared/ListPageLayout';
import type { TableColumn } from '../../shared/ListPageLayout';
import { actionColumn } from '../../shared/ActionButtons';
import { DisciplinaryRecordCreate } from './DisciplinaryRecordCreate';
import { DisciplinaryRecordEdit } from './DisciplinaryRecordEdit';
import { useFlows } from '../../flows/FlowProvider';

// ---------------------------------------------------------------------------
// Types
// ---------------------------------------------------------------------------

type DisciplinaryRecordRecord = {
  id: string | number;
  incidentDate: string;
  type: string;
  description: string;
  actionTaken?: string;
  issuedBy?: string;
  acknowledgedByEmployee: boolean;
  status: string;
  appealNote?: string;
  resolutionNote?: string;
  hrReviewerId?: number;
  hrManagerResolverId?: number;
  employeeId: string;
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
  { field: 'incidentDate', header: 'Incident Date', sortable: true, filterType: 'datepicker', html: (row: Record<string, unknown>) => row.incidentDate ? new Date(String(row.incidentDate)).toLocaleDateString() : '—' },
  { field: 'type', header: 'Type', sortable: true, filterType: 'radio', filterOptions: [{ label: 'VerbalWarning', value: '0' }, { label: 'WrittenWarning', value: '1' }, { label: 'FinalWarning', value: '2' }, { label: 'Suspension', value: '3' }, { label: 'Termination', value: '4' }], html: (row: Record<string, unknown>) => { const m: Record<string, string> = {'0': 'VerbalWarning', '1': 'WrittenWarning', '2': 'FinalWarning', '3': 'Suspension', '4': 'Termination'}; return m[String(row.type ?? '')] ?? String(row.type ?? '\u2014'); } },
  { field: 'description', header: 'Description', sortable: true, searchable: true, filterType: 'text' },
  { field: 'actionTaken', header: 'Action Taken', sortable: true, searchable: true, filterType: 'text' },
  { field: 'issuedBy', header: 'Issued By', sortable: true, searchable: true, filterType: 'text' },
  { field: 'acknowledgedByEmployee', header: 'Acknowledged By Employee', sortable: true, filterType: 'checkbox', filterOptions: [{ label: 'Yes', value: 'true' }, { label: 'No', value: 'false' }], html: (row: Record<string, unknown>) => row.acknowledgedByEmployee ? '<span style="color:#2e7d32;font-weight:600">Yes</span>' : '<span style="color:#999">No</span>' },
  { field: 'status', header: 'Status', sortable: true, filterType: 'radio', filterOptions: [{ label: 'Open', value: '0' }, { label: 'UnderReview', value: '1' }, { label: 'Appealed', value: '2' }, { label: 'Resolved', value: '3' }, { label: 'Closed', value: '4' }], html: (row: Record<string, unknown>) => { const m: Record<string, string> = {'0': 'Open', '1': 'UnderReview', '2': 'Appealed', '3': 'Resolved', '4': 'Closed'}; return m[String(row.status ?? '')] ?? String(row.status ?? '\u2014'); } },
  { field: 'appealNote', header: 'Appeal Note', sortable: true, searchable: true, filterType: 'text' },
  { field: 'resolutionNote', header: 'Resolution Note', sortable: true, searchable: true, filterType: 'text' },
  { field: 'hrReviewerId', header: 'Hr Reviewer Id', sortable: true },
  { field: 'hrManagerResolverId', header: 'Hr Manager Resolver Id', sortable: true },
  { field: 'employeeId', header: 'Personel', sortable: true },
];

// ---------------------------------------------------------------------------
// DisciplinaryRecordList
// ---------------------------------------------------------------------------

export const DisciplinaryRecordList: React.FC = () => {
  const list = useListQuery<DisciplinaryRecordRecord>({ resource: 'DisciplinaryRecord' });
  const [showCreate, setShowCreate] = useState(false);
  const [editRecord, setEditRecord] = useState<DisciplinaryRecordRecord | null>(null);
  const [selectedRows, setSelectedRows] = useState<DisciplinaryRecordRecord[]>([]);
  const { triggerFlows } = useFlows();
  const del = useDeleteMutation('DisciplinaryRecord', (ids) => { ids.forEach(id => triggerFlows('delete', 'DisciplinaryRecord', { id })); });


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
        title="DisciplinaryRecord"
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
        onSelectionChange={(rows) => setSelectedRows(rows as DisciplinaryRecordRecord[])}
        onCrudAction={handleCrudAction}
        headerActions={<>
          {selectedRows.length > 0 && (
            <TkButton label={`Delete (${selectedRows.length})`} variant="danger" onTkClick={() => del.requestDelete(selectedRows.map(r => r.id), `${selectedRows.length} record(s)`)} />
          )}

          <TkButton label="+ Create DisciplinaryRecord" variant="primary" onTkClick={() => setShowCreate(true)} />
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
      <DisciplinaryRecordCreate open={showCreate} onClose={() => setShowCreate(false)} onSuccess={list.invalidate} />
      <DisciplinaryRecordEdit record={editRecord} onClose={() => setEditRecord(null)} onSuccess={list.invalidate} />

    </>
  );
};

