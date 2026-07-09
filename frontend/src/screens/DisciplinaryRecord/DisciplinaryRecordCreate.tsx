import React, { useState } from 'react';
import { useMutation } from '@tanstack/react-query';
import { TkButton, TkDatepicker, TkInput, TkSelect } from '@takeoff-ui/react';
import { dataProvider } from '../../dataProvider';
import { overlayStyle, modalStyle } from '../../styles';
import { LookupSelect } from '../../shared/LookupSelect';
import { useFlows } from '../../flows/FlowProvider';

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
};

interface DisciplinaryRecordCreateProps {
  open: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export const DisciplinaryRecordCreate: React.FC<DisciplinaryRecordCreateProps> = ({ open, onClose, onSuccess }) => {
  const [form, setForm] = useState<Partial<DisciplinaryRecordRecord>>({});
  const setField = (name: string, value: unknown) => setForm((p) => ({ ...p, [name]: value }));
  const { triggerFlows } = useFlows();

  const mutation = useMutation({
    mutationFn: (values: Partial<DisciplinaryRecordRecord>) => dataProvider.create('DisciplinaryRecord', values),
    onSuccess: (_data, values) => { triggerFlows('create', 'DisciplinaryRecord', values as Record<string, unknown>); onSuccess(); onClose(); setForm({}); },
    onError: (err: Error) => { window.dispatchEvent(new CustomEvent('app-toast', { detail: { type: 'error', message: err.message } })); },
  });

  if (!open) return null;

  return (
    <div style={overlayStyle} onClick={onClose}>
      <div style={modalStyle} onClick={(e) => e.stopPropagation()}>
        <div style={{ padding: '20px 28px 0', flexShrink: 0, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <h2 style={{ margin: 0, fontSize: 18, fontWeight: 700 }}>Create DisciplinaryRecord</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', fontSize: 20, cursor: 'pointer', color: '#666', padding: '4px 8px', borderRadius: 4 }} onMouseOver={(e) => (e.currentTarget.style.color = '#333')} onMouseOut={(e) => (e.currentTarget.style.color = '#666')}>✕</button>
        </div>
        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(form); }} style={{ display: 'flex', flexDirection: 'column', flex: 1, overflow: 'hidden' }}>
          <div style={{ flex: 1, overflowY: 'auto', padding: '20px 28px' }}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
                <div>
                  <TkDatepicker label="Incident Date *" value={String(form.incidentDate ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('incidentDate', v))(e.detail)} />
                </div>
                <div>
                  <LookupSelect label="Type *" value={String(form.type ?? '')} onChange={(v) => setField('type', v ? Number(v) : null)} searchable={false} options={[{ label: 'VerbalWarning', value: '0' }, { label: 'WrittenWarning', value: '1' }, { label: 'FinalWarning', value: '2' }, { label: 'Suspension', value: '3' }, { label: 'Termination', value: '4' }]} />
                </div>
                <div>
                  <TkInput mode="text" label="Description *" value={String(form.description ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('description', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Action Taken" value={String(form.actionTaken ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('actionTaken', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Issued By" value={String(form.issuedBy ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('issuedBy', v))(e.detail)} />
                </div>
                <div>
                  <label style={{ display: 'flex', alignItems: 'center', gap: 8, fontSize: 14 }}>
                    <input type="checkbox" checked={!!form.acknowledgedByEmployee} onChange={(e) => ((v) => setField('acknowledgedByEmployee', v))(e.target.checked)} style={{ width: 16, height: 16 }} />
                    Acknowledged By Employee *
                  </label>
                </div>
                <div>
                  <LookupSelect label="Status *" value={String(form.status ?? '')} onChange={(v) => setField('status', v ? Number(v) : null)} searchable={false} options={[{ label: 'Open', value: '0' }, { label: 'UnderReview', value: '1' }, { label: 'Appealed', value: '2' }, { label: 'Resolved', value: '3' }, { label: 'Closed', value: '4' }]} />
                </div>
                <div>
                  <TkInput mode="text" label="Appeal Note" value={String(form.appealNote ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('appealNote', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="text" label="Resolution Note" value={String(form.resolutionNote ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('resolutionNote', v))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Hr Reviewer Id" value={String(form.hrReviewerId ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('hrReviewerId', Number(v)))(e.detail)} />
                </div>
                <div>
                  <TkInput mode="number" label="Hr Manager Resolver Id" value={String(form.hrManagerResolverId ?? '')} onTkChange={(e: CustomEvent) => ((v) => setField('hrManagerResolverId', Number(v)))(e.detail)} />
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
