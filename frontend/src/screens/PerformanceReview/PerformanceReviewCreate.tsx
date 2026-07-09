import React, { useState } from 'react';
import { useMutation } from '@tanstack/react-query';
import { TkButton, TkInput, TkSelect } from '@takeoff-ui/react';
import { dataProvider } from '../../dataProvider';
import { overlayStyle, modalStyle } from '../../styles';
import { LookupSelect } from '../../shared/LookupSelect';
import { useFlows } from '../../flows/FlowProvider';

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
};

interface PerformanceReviewCreateProps {
  open: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const PerformanceReviewCreate: React.FC<PerformanceReviewCreateProps> = ({ open, onClose, onSuccess }) => {
  const [form, setForm] = useState<Partial<PerformanceReviewRecord>>({});
  const setField = (name: string, value: unknown) => setForm((p) => ({ ...p, [name]: value }));
  const { triggerFlows } = useFlows();

  const mutation = useMutation({
    mutationFn: (values: Partial<PerformanceReviewRecord>) => dataProvider.create('PerformanceReview', values),
    onSuccess: (_data, values) => { triggerFlows('create', 'PerformanceReview', values as Record<string, unknown>); onSuccess(); onClose(); setForm({}); },
    onError: (err: Error) => { window.dispatchEvent(new CustomEvent('app-toast', { detail: { type: 'error', message: err.message } })); },
  });

  if (!open) return null;

  return (
    <div style={overlayStyle} onClick={onClose}>
      <div style={modalStyle} onClick={(e) => e.stopPropagation()}>
        <div style={{ padding: '20px 28px 0', flexShrink: 0, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <h2 style={{ margin: 0, fontSize: 18, fontWeight: 700 }}>Create PerformanceReview</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', fontSize: 20, cursor: 'pointer', color: '#666', padding: '4px 8px', borderRadius: 4 }} onMouseOver={(e) => (e.currentTarget.style.color = '#333')} onMouseOut={(e) => (e.currentTarget.style.color = '#666')}>✕</button>
        </div>
        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(form); }} style={{ display: 'flex', flexDirection: 'column', flex: 1, overflow: 'hidden' }}>
          <div style={{ flex: 1, overflowY: 'auto', padding: '20px 28px' }}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                <div>
                  <TkInput mode="text" label="Review Period *" value={String(form.reviewPeriod ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('reviewPeriod', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Review Year *" value={String(form.reviewYear ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('reviewYear', Number(v)))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Review Type *" value={String(form.reviewType ?? '')} onChange={(v) => setField('reviewType', v ? Number(v) : null)} searchable={false} options={[{ label: 'Annual', value: '0' }, { label: 'MidYear', value: '1' }, { label: 'Probation', value: '2' }]} />
                </div>
                <div>
                  <LookupSelect label="Status *" value={String(form.status ?? '')} onChange={(v) => setField('status', v ? Number(v) : null)} searchable={false} options={[{ label: 'Draft', value: '0' }, { label: 'SelfAssessmentPending', value: '1' }, { label: 'ManagerReviewPending', value: '2' }, { label: 'PeerReviewPending', value: '3' }, { label: 'HRReviewPending', value: '4' }, { label: 'Completed', value: '5' }, { label: 'Cancelled', value: '6' }]} />
                </div>
                <div>
                  <TkInput mode="number" label="Self Assessment Score" value={String(form.selfAssessmentScore ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('selfAssessmentScore', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Self Assessment Notes" value={String(form.selfAssessmentNotes ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('selfAssessmentNotes', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Manager Score" value={String(form.managerScore ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('managerScore', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Manager Notes" value={String(form.managerNotes ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('managerNotes', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Overall Score" value={String(form.overallScore ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('overallScore', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Hr Notes" value={String(form.hrNotes ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('hrNotes', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Revision Note" value={String(form.revisionNote ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('revisionNote', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Manager Reviewer Id" value={String(form.managerReviewerId ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('managerReviewerId', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Hr Reviewer Id" value={String(form.hrReviewerId ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('hrReviewerId', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Peer Reviewers Assigned By" value={String(form.peerReviewersAssignedBy ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('peerReviewersAssignedBy', Number(v)))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Personel *" resource="Employee" value={String(form.employeeId ?? '')} onChange={(v) => setField('employeeId', v)} displayField="employeeNumber" />
                </div>
            </div>
          </div>
          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 8, padding: '16px 28px', borderTop: '1px solid #e8e8e8', flexShrink: 0, background: '#fff' }}>
            <TkButton label="Cancel" variant="secondary" onTkClick={onClose} />
            <TkButton label={mutation.isPending ? 'Saving…' : 'Create'} variant="primary" mode="submit" disabled={mutation.isPending} />
          </div>
        </form>
      </div>
    </div>
  );
};
