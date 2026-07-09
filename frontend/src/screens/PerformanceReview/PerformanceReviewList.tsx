import React, { useState, useCallback } from 'react';
import { TkButton } from '@takeoff-ui/react';
import { useListQuery } from '../../shared/useListQuery';
import { useDeleteMutation } from '../../shared/useDeleteMutation';
import { DeleteConfirmDialog } from '../../shared/DeleteConfirmDialog';
import { ListPageLayout } from '../../shared/ListPageLayout';
import type { TableColumn } from '../../shared/ListPageLayout';
import { actionColumn } from '../../shared/ActionButtons';
import { PerformanceReviewCreate } from './PerformanceReviewCreate';
import { PerformanceReviewEdit } from './PerformanceReviewEdit';
import { useFlows } from '../../flows/FlowProvider';

// ---------------------------------------------------------------------------
// Types
// ---------------------------------------------------------------------------

type PerformanceReviewRecord = {
  id: string | number;
  reviewPeriod: string;
  reviewYear: number;
  reviewType: string;
  status: string;
  selfAssessmentScore?: number;
  selfAssessmentNotes?: string;
  managerScore?: number;
  managerNotes?: string;
  overallScore?: number;
  hrNotes?: string;
  revisionNote?: string;
  managerReviewerId?: number;
  hrReviewerId?: number;
  peerReviewersAssignedBy?: number;
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
  { field: 'reviewPeriod', header: 'Review Period', sortable: true, searchable: true, filterType: 'text' },
  { field: 'reviewYear', header: 'Review Year', sortable: true },
  { field: 'reviewType', header: 'Review Type', sortable: true, filterType: 'radio', filterOptions: [{ label: 'Annual', value: '0' }, { label: 'MidYear', value: '1' }, { label: 'Probation', value: '2' }], html: (row: Record<string, unknown>) => { const m: Record<string, string> = {'0': 'Annual', '1': 'MidYear', '2': 'Probation'}; return m[String(row.reviewType ?? '')] ?? String(row.reviewType ?? '\u2014'); } },
  { field: 'status', header: 'Status', sortable: true, filterType: 'radio', filterOptions: [{ label: 'Draft', value: '0' }, { label: 'SelfAssessmentPending', value: '1' }, { label: 'ManagerReviewPending', value: '2' }, { label: 'PeerReviewPending', value: '3' }, { label: 'HRReviewPending', value: '4' }, { label: 'Completed', value: '5' }, { label: 'Cancelled', value: '6' }], html: (row: Record<string, unknown>) => { const m: Record<string, string> = {'0': 'Draft', '1': 'SelfAssessmentPending', '2': 'ManagerReviewPending', '3': 'PeerReviewPending', '4': 'HRReviewPending', '5': 'Completed', '6': 'Cancelled'}; return m[String(row.status ?? '')] ?? String(row.status ?? '\u2014'); } },
  { field: 'selfAssessmentScore', header: 'Self Assessment Score', sortable: true },
  { field: 'selfAssessmentNotes', header: 'Self Assessment Notes', sortable: true, searchable: true, filterType: 'text' },
  { field: 'managerScore', header: 'Manager Score', sortable: true },
  { field: 'managerNotes', header: 'Manager Notes', sortable: true, searchable: true, filterType: 'text' },
  { field: 'overallScore', header: 'Overall Score', sortable: true },
  { field: 'hrNotes', header: 'Hr Notes', sortable: true, searchable: true, filterType: 'text' },
  { field: 'revisionNote', header: 'Revision Note', sortable: true, searchable: true, filterType: 'text' },
  { field: 'managerReviewerId', header: 'Manager Reviewer Id', sortable: true },
  { field: 'hrReviewerId', header: 'Hr Reviewer Id', sortable: true },
  { field: 'peerReviewersAssignedBy', header: 'Peer Reviewers Assigned By', sortable: true },
  { field: 'employeeId', header: 'Personel', sortable: true },
];

// ---------------------------------------------------------------------------
// PerformanceReviewList
// ---------------------------------------------------------------------------

export const PerformanceReviewList: React.FC = () => {
  const list = useListQuery<PerformanceReviewRecord>({ resource: 'PerformanceReview' });
  const [showCreate, setShowCreate] = useState(false);
  const [editRecord, setEditRecord] = useState<PerformanceReviewRecord | null>(null);
  const [selectedRows, setSelectedRows] = useState<PerformanceReviewRecord[]>([]);
  const { triggerFlows } = useFlows();
  const del = useDeleteMutation('PerformanceReview', (ids) => { ids.forEach(id => triggerFlows('delete', 'PerformanceReview', { id })); });


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
        title="PerformanceReview"
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
        onSelectionChange={(rows) => setSelectedRows(rows as PerformanceReviewRecord[])}
        onCrudAction={handleCrudAction}
        headerActions={<>
          {selectedRows.length > 0 && (
            <TkButton label={`Delete (${selectedRows.length})`} variant="danger" onTkClick={() => del.requestDelete(selectedRows.map(r => r.id), `${selectedRows.length} record(s)`)} />
          )}

          <TkButton label="+ Create PerformanceReview" variant="primary" onTkClick={() => setShowCreate(true)} />
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
      <PerformanceReviewCreate open={showCreate} onClose={() => setShowCreate(false)} onSuccess={list.invalidate} />
      <PerformanceReviewEdit record={editRecord} onClose={() => setEditRecord(null)} onSuccess={list.invalidate} />

    </>
  );
};

